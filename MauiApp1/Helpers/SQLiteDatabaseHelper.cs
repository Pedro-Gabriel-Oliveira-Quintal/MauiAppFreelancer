using MauiApp1.Models;
using SQLite;

namespace MauiApp1.Helpers
{
    public class SQLiteDatabaseHelper
    {
        readonly SQLiteAsyncConnection _conn; 

        public SQLiteDatabaseHelper(string path) 
        {
            _conn = new SQLiteAsyncConnection(path);
            _conn.ExecuteAsync("PRAGMA foreign_keys = ON;").Wait();

            
            _conn.CreateTableAsync<Usuario>().Wait();
            _conn.ExecuteAsync(@"
                CREATE TABLE IF NOT EXISTS Perfil (
                    idPerfil INTEGER PRIMARY KEY AUTOINCREMENT,
                    idUsuario INTEGER NOT NULL,
                    nomeExibicao TEXT(100),
                    documentos TEXT,
                    fotoPerfil TEXT,
                    biografia TEXT,
                    FOREIGN KEY(idUsuario) REFERENCES Usuario(idUsuario)
                );
            ").Wait();
            _conn.ExecuteAsync(@"
               CREATE TABLE IF NOT EXISTS Empresa (
                    idEmpresa INTEGER PRIMARY KEY,
                    login TEXT(80) UNIQUE NOT NULL,
                    senha TEXT(20) NOT NULL,
                    FOREIGN KEY (idEmpresa) REFERENCES Usuario(idUsuario)
                );
            ").Wait();
            _conn.ExecuteAsync(@"
                CREATE TABLE IF NOT EXISTS Autonomo (
                    idAutonomo INTEGER PRIMARY KEY,
                    login TEXT(80) UNIQUE NOT NULL,
                    senha TEXT(20) NOT NULL,
                    FOREIGN KEY (idAutonomo) REFERENCES Usuario(idUsuario) ON DELETE CASCADE
                );
            ").Wait();
            _conn.ExecuteAsync(@"
                CREATE TABLE IF NOT EXISTS UsuarioAdministrador (
                    idAdmin INTEGER PRIMARY KEY,
                    login TEXT(80) UNIQUE NOT NULL,
                    senha TEXT(20) NOT NULL,
                    FOREIGN KEY (idAdmin) REFERENCES Usuario(idUsuario) ON DELETE CASCADE
                );
            ").Wait();
            _conn.ExecuteAsync(@"
                CREATE TABLE IF NOT EXISTS Postagem (
                    idPostagem INTEGER PRIMARY KEY AUTOINCREMENT,
                    idUsuario INTEGER NOT NULL,
                    conteudo TEXT NOT NULL,
                    data_publicacao DATETIME DEFAULT CURRENT_TIMESTAMP,
                    FOREIGN KEY (idUsuario) REFERENCES Usuario(idUsuario) ON DELETE CASCADE
                );
            ").Wait();
            _conn.ExecuteAsync(@"
                CREATE TABLE IF NOT EXISTS Comentario(
                    idComentario INTEGER PRIMARY KEY AUTOINCREMENT,
                    idPostagem INTEGER NOT NULL,
                    idUsuario INTEGER NOT NULL,
                    conteudo TEXT NOT NULL,
                    data_comentario DATETIME DEFAULT CURRENT_TIMESTAMP,
                    FOREIGN KEY (idPostagem) REFERENCES Postagem(idPostagem) ON DELETE CASCADE,
                    FOREIGN KEY (idUsuario) REFERENCES Usuario(idUsuario) ON DELETE CASCADE
                );
            ").Wait();
            _conn.ExecuteAsync(@"
                CREATE TABLE IF NOT EXISTS Notificacao (
                    idNotificacao INTEGER PRIMARY KEY AUTOINCREMENT,
                    idUsuarioDestinatario INTEGER NOT NULL,
                    idUsuarioRemetente INTEGER,
                    tipo TEXT CHECK (tipo IN ('banimento', 'atualizacao', 'comentario', 'outro')) NOT NULL,
                    mensagem TEXT NOT NULL,
                    data_notificacao DATETIME DEFAULT CURRENT_TIMESTAMP,
                    lida BOOLEAN DEFAULT FALSE,
                    FOREIGN KEY (idUsuarioDestinatario) REFERENCES Usuario(idUsuario) ON DELETE CASCADE,
                    FOREIGN KEY (idUsuarioRemetente) REFERENCES Usuario(idUsuario) ON DELETE SET NULL
                );
            ").Wait();
        }

        //Usuário
        public Task<int> InsertUsuario(Usuario usuario)
        {
            return _conn.InsertAsync(usuario);
        }
        public Task<List<Usuario>> UpdateUsuario(Usuario usuario)
        {
            string sql = "UPDATE Usuario SET nome=?, dataNascimento=?, endereco=?, bairro=?, cep=?, telefone=?, email=?, CPF_CNPJ=?, tipo=? WHERE idUsuario=?";

            return _conn.QueryAsync<Usuario>(sql, usuario.nome, usuario.dataNascimento, usuario.endereco, usuario.bairro, usuario.cep, usuario.telefone, usuario.email, usuario.CPF_CNPJ, usuario.tipo, usuario.idUsuario);
        }
        public Task<int> DeleteUsuario(int idUsuario)
        {
            return _conn.Table<Usuario>().DeleteAsync(i => i.idUsuario == idUsuario);
        }

        //Autonomo
        public Task<int> InsertAutonomo(Autonomo autonomo)
        {
            return _conn.InsertAsync(autonomo);
        }

        //Empresa
        public Task<int> InsertEmpresa(Empresa empresa)
        {
            return _conn.InsertAsync(empresa);
        }

        //Postagem
        public Task<int> InsertPostagem(Postagem postagem) 
        {
            return _conn.InsertAsync(postagem);
        }
        public Task<List<Postagem>> UpdatePostagem(Postagem postagem) 
        {
            string sql = "UPDATE Postagem SET conteudo=? WHERE idPostagem=?";

            return _conn.QueryAsync<Postagem>(sql, postagem.conteudo, postagem.idPostagem);
        }
        public Task<int> DeletePostagem(int idPostagem) 
        {
            return _conn.Table<Postagem>().DeleteAsync(i => i.idPostagem == idPostagem);
        }

        //Comentário
        public Task<int> InsertComentario(Comentario comentario) 
        {
            return _conn.InsertAsync(comentario);
        }
        public Task<List<Comentario>> UpdateComentario(Comentario comentario) 
        {
            string sql = "UPDATE Comentario SET conteudo=? WHERE idComentario=?";

            return _conn.QueryAsync<Comentario>(sql, comentario.conteudo, comentario.idComentario);
        }
        public Task<int> DeleteComentario(int idComentario) 
        {
            return _conn.Table<Comentario>().DeleteAsync(i => i.idComentario == idComentario);
        }

        //Perfil
        public Task<int> InsertPerfil(Perfil perfil) 
        {
            return _conn.InsertAsync(perfil);
        }
        public Task<List<Perfil>> UpdatePerfil(Perfil perfil) 
        {
            string sql = "UPDATE Perfil SET nomeExibicao=?, documentos=?, fotoPerfil=?, biografia=? WHERE idPerfil=?";

            return _conn.QueryAsync<Perfil>(sql, perfil.nomeExibicao, perfil.documentos, perfil.fotoPerfil, perfil.biografia, perfil.idPerfil);
        }
        public Task<int> DeletePerfil(int idPerfil) 
        {
            return _conn.Table<Perfil>().DeleteAsync(i => i.idPerfil == idPerfil);
        }

        //Notificação
        public Task<List<Notificacao>> MarcarNotificacaoLida(int idNotificacaoLida)
        {
            string sql = "UPDATE Notificacao SET lido = 1 WHERE idNotificacao = ?";
            return _conn.QueryAsync<Notificacao>(sql, idNotificacaoLida);
        }

        //Gets
        public Task<List<Postagem>> GetPostagem() 
        {
            return _conn.Table<Postagem>().ToListAsync();
        }


        //Search
        public Task<List<Postagem>> Search(string q) 
        {
            string sql = "SELECT * FROM Postagem WHERE conteudo LIKE '%" + q + "%'";

            return _conn.QueryAsync<Postagem>(sql);
        }

        public async Task<Usuario> GetUsuarioPorEmail(string email)
        {
            string sql = "SELECT * FROM Usuario WHERE email = ?";
            var result = await _conn.QueryAsync<Usuario>(sql, email);

            return result.FirstOrDefault();
        }

        public Task<Perfil> GetPerfilPorUsuarioId(int idUsuario)
        {
            return _conn.Table<Perfil>().Where(p => p.idUsuario == idUsuario).FirstOrDefaultAsync();
        }
    }
}
