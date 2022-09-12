using System;
using System.IO;
using System.Threading;
using System.Drawing;

namespace didaticos.redimensionador
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Iniciando redimensionador!");

            Thread thread = new Thread(Redimensionar);
            thread.Start();

            Console.Read();
        }

        static void Redimensionar()
        {
            #region "Diretorios"
            string diretorio_entrada = "Arquivos_Entrada";
            string diretorio_redimensionado = "Arquivos_Redimensionados";
            string diretorio_finalizado = "Arquivos_Finalizados";

            if (!Directory.Exists(diretorio_entrada))
            {
                Directory.CreateDirectory(diretorio_entrada);
            }

            if (!Directory.Exists(diretorio_redimensionado))
            {
                Directory.CreateDirectory(diretorio_redimensionado);
            }

            if (!Directory.Exists(diretorio_finalizado))
            {
                Directory.CreateDirectory(diretorio_finalizado);
            }

            #endregion
            FileStream fileStream;
            FileInfo fileInfo;

            while (true)
            {
                // Meu programa vai olhar para a pasta de entrada
                // Se tiver arquivo, ele irá redimensionar
                var arquivosEntrada = Directory.EnumerateFiles(diretorio_entrada);

                // Ler o tamanho que irá redimensionar
                int novaAltura = 200;

                foreach (var arquivo in arquivosEntrada)
                {
                    fileStream = new FileStream(arquivo, FileMode.Open, FileAccess.ReadWrite, FileShare.ReadWrite);
                    fileInfo = new FileInfo(arquivo);

                    string caminho = Environment.CurrentDirectory + @"\" + diretorio_redimensionado 
                        + @"\" + DateTime.Now.Millisecond.ToString() + "_" + fileInfo.Name;

                    // Redimensiona + //  Copia os arquivos redimensionados para a pasta de redimensionados
                    Redimensionador(Image.FromStream(fileStream), novaAltura, caminho);

                    // Fecha o arquivo
                    fileStream.Close();

                    // Move o arquivo de entrada para a pasta de finalizados
                    string caminhoFinalizado = Environment.CurrentDirectory + @"\" + diretorio_finalizado + @"\" + fileInfo.Name;
                    fileInfo.MoveTo(caminhoFinalizado);
                }

                Thread.Sleep(new TimeSpan(0,0,3));
            }
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="imagem">Imagem a ser redimensionada</param>
        /// <param name="altura">Altura que desejamos redimensionar</param>
        /// <param name="caminho">Caminho aonde irmos gravar o arquivo redimensionado</param>
        /// <returns></returns>
        static void Redimensionador(Image imagem, int altura, string caminho)
        {
            double ratio = (double)altura / imagem.Height;
            int novaLargura = (int)(imagem.Width * ratio);
            int novaAltura = (int)(imagem.Height * ratio);

            Bitmap novaImagem = new Bitmap(novaLargura, novaAltura);
            
            using(Graphics g = Graphics.FromImage(novaImagem))
            {
                g.DrawImage(imagem, 0, 0, novaLargura, novaAltura);
            }

            novaImagem.Save(caminho);
            imagem.Dispose();
        }
    }
}