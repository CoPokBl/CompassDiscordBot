using CompassApi;
using GeneralPurposeLib;
using MySql.Data.MySqlClient;
using Newtonsoft.Json;

namespace CompassDiscordBot.Data; 

public static class StorageManager {
    private static string _connectionString = "";
    
    public static void Init() {
        Logger.Info($"[MySQL] Connecting to {GlobalConfig.Config["mysql_host"].Text} as {GlobalConfig.Config["mysql_user"].Text}...");
        _connectionString = $"server={GlobalConfig.Config["mysql_host"].Text};" +
                            $"userid={GlobalConfig.Config["mysql_user"].Text};" +
                            $"password={GlobalConfig.Config["mysql_password"].Text};" +
                            $"database={GlobalConfig.Config["mysql_database"].Text}";
        CreateTables().Wait();
        Logger.Info("Initialised MySQL");
    }
    
    private static async Task CreateTables() {
        await SendMySqlStatement(@"CREATE TABLE IF NOT EXISTS compass_users(
                           id VARCHAR(64) primary key,
                           login_state VARCHAR(512))");
    }
    
    private static async Task SendMySqlStatement(string statement, params MySqlParameter[] param) {
        await MySqlHelper.ExecuteNonQueryAsync(_connectionString, statement, param);
    }

    public static void Deinit() {
        Logger.Info("De-initialised MySQL");
    }

    public static async Task<CompassLoginState?> GetUserLoginState(string id) {
        await using MySqlDataReader reader = await MySqlHelper.ExecuteReaderAsync(_connectionString, "SELECT * FROM compass_users WHERE id=@id",
            new MySqlParameter("@id", id));
        if (!reader.Read()) {
            return null;
        }
        string jsonState = reader.GetString("login_state");
        reader.Close();
        return JsonConvert.DeserializeObject<CompassLoginState>(jsonState);
    }
    
    public static async Task SetUserLoginState(string id, CompassLoginState state) {
        await SendMySqlStatement("INSERT INTO compass_users (id, login_state) VALUES (@id, @state) ON DUPLICATE KEY UPDATE login_state=@state",
            new MySqlParameter("@id", id),
            new MySqlParameter("@state", JsonConvert.SerializeObject(state)));
    }
    
    
}