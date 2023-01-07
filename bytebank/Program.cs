using System.Reflection.Metadata.Ecma335;
using System.Xml;

namespace ByteBank1
{

    public class Program
    {

        static void ShowMenu()
        {
            Console.WriteLine();
            Console.WriteLine("   ----------------------------------------");
            Console.WriteLine("            Bem vindo ao ByteBank");
            Console.WriteLine("   ----------------------------------------");
            Console.WriteLine("   1 - Inserir novo Cliente");
            Console.WriteLine("   2 - Deletar um Cliente");
            Console.WriteLine("   3 - Listar todas as contas registradas");
            Console.WriteLine("   4 - Detalhes de um Cliente");
            Console.WriteLine("   5 - Quantia armazenada no banco");
            Console.WriteLine("   6 - Manipular a conta ");
            Console.WriteLine("   0 - Para sair do programa");
            Console.WriteLine("   ----------------------------------------");
            Console.WriteLine();
            Console.Write("   Digite a opção desejada: ");
        }

        static void RegistraUsuarios(List<string> cpfs, List<string> titulares, List<string> senhas, List<double> saldos)
        {

                int option;
                Console.WriteLine();
                Console.Write("   Digite o cpf: ");
                cpfs.Add(Console.ReadLine());
                Console.Write("   Digite o nome: ");
                titulares.Add(Console.ReadLine());
                Console.Write("   Digite a senha: ");
                senhas.Add(Console.ReadLine());
                saldos.Add(0);
                Console.Clear();
               
               

        }

        static void DeletaUsuarios(List<string> cpfs, List<string> titulares, List<string> senhas, List<double> saldos)
        {
            Console.WriteLine("");
            Console.Write("   Digite o cpf: ");
            Console.WriteLine();
            string cpfParaDeletar = Console.ReadLine();
            int indexParaDeletar = cpfs.FindIndex(cpf => cpf == cpfParaDeletar);

            if (indexParaDeletar == -1)
            {
                Console.WriteLine();
                Console.WriteLine("   Não foi possível deletar esta Conta");
                Console.WriteLine("   MOTIVO: Conta não encontrada.");
                Console.WriteLine();
            }

            cpfs.Remove(cpfParaDeletar);
            titulares.RemoveAt(indexParaDeletar);
            senhas.RemoveAt(indexParaDeletar);
            saldos.RemoveAt(indexParaDeletar);
            Console.WriteLine();
            Console.WriteLine("   Conta deletada com sucesso");
            Console.WriteLine();
        }

        static void ListarTodasAsContas(List<string> cpfs, List<string> titulares, List<double> saldos)
        {
            for (int i = 0; i < cpfs.Count; i++)
            {
                ApresentaConta(i, cpfs, titulares, saldos);
            }
        }

        static void ApresentarUsuario(List<string> cpfs, List<string> titulares, List<double> saldos)
        {
            Console.Write("   Digite o cpf: ");
            string cpfParaApresentar = Console.ReadLine();
            int indexParaApresentar = cpfs.FindIndex(cpf => cpf == cpfParaApresentar);

            if (indexParaApresentar == -1)
            {
                Console.WriteLine("   Não foi possível apresentar esta Conta");
                Console.WriteLine("   MOTIVO: Conta não encontrada.");
            }

            ApresentaConta(indexParaApresentar, cpfs, titulares, saldos);
        }

        static void ApresentarValorAcumulado(List<double> saldos)
        {
            Console.WriteLine($"   Total acumulado no banco: {saldos.Sum()}");
            
        }

        static void ApresentaConta(int index, List<string> cpfs, List<string> titulares, List<double> saldos)
        {
            Console.WriteLine($"   CPF = {cpfs[index]} | Titular = {titulares[index]} | Saldo = R${saldos[index]:F2}");
        }
        static int BuscaCPF(List<string> cpfs, string cpfToFind)
        {
            int indexCPF = cpfs.FindIndex(cpf => cpf == cpfToFind);
            return indexCPF;
        }

        static bool DepositarValor(List<string> cpfs, List<string> senhas, List<double> saldos, string cpfDepositar = "", double valorDeposito = 0)
        {
            bool ctlMsg = false;
            if (cpfDepositar == "")
            {
                Console.WriteLine("   ----------------------------------");
                Console.WriteLine("               Depósito");
                Console.WriteLine("   ----------------------------------");
                Console.Write("   Digite o CPF para realizar o Depósito: ");
                cpfDepositar = Console.ReadLine();
                ctlMsg = true;
            }
            int indexCpfDepositar = BuscaCPF(cpfs, cpfDepositar);
            if (indexCpfDepositar == -1)
            {
                Console.WriteLine("\n  Tente novamente");
                Console.WriteLine("   Conta não encontrada.\n");
            }
            else
            {
                if (valorDeposito == 0d)
                {
                    Console.Write("   Informe o Valor a ser Depositado: R$ ");
                    valorDeposito = double.Parse(Console.ReadLine());
                }
                if (valorDeposito > 0d)
                {
                    saldos[indexCpfDepositar] += valorDeposito;
                    if (ctlMsg == true)
                    {
                        Console.WriteLine("\n   Depósito realizado com Sucesso!\n");
                    }
                    return true;
                }
                else
                {
                    Console.WriteLine("\n  Tente novamente");
                    Console.WriteLine("   Valor de depósito inválido!\n");
                }
            }
            return false;
        }

        static void TransferirValor(List<string> cpfs, List<string> senhas, List<double> saldos)
        {
            bool ctlTransactionSaque = false;
            bool ctlTransactionDeposito = false;
            Console.WriteLine("   ----------------------------------");
            Console.WriteLine("             Transferência");
            Console.WriteLine("   ----------------------------------");
            Console.Write("   Informe o CPF da conta a ser debitada:: ");
            string cpfSacar = Console.ReadLine();
            int indexCpfSacar = BuscaCPF(cpfs, cpfSacar);
            if (indexCpfSacar == -1)
            {
                Console.WriteLine("\n  Tente novamente");
                Console.WriteLine("   Conta de Origem NÃO encontrada.\n");
                return;
            }
            Console.Write("   Informe o CPF de Destino da Transferência: ");
            string cpfDepositar = Console.ReadLine();
            int indexCpfDepositar = BuscaCPF(cpfs, cpfDepositar);
            if (indexCpfDepositar == -1)
            {
                Console.WriteLine("\n  Tente novamente");
                Console.WriteLine("   Conta de Destino NÃO encontrada.\n");
                return;
            }
            Console.Write("   Informe o valor da Transferência: R$ ");
            double valorTransferencia = double.Parse(Console.ReadLine());
            ctlTransactionSaque = SacarValor(cpfs, senhas, saldos, cpfSacar, valorTransferencia);
            if (ctlTransactionSaque)
            {
                ctlTransactionDeposito = DepositarValor(cpfs, senhas, saldos, cpfDepositar, valorTransferencia);
                if (ctlTransactionDeposito == false)
                {
                    DepositarValor(cpfs, senhas, saldos, cpfSacar, valorTransferencia);
                }
            }
            if (ctlTransactionDeposito == true && ctlTransactionSaque == true)
            {
                Console.WriteLine("\n   Transferência Realizada com Sucesso!\n");
            }
        }
        static bool ValidaSenha(List<string> cpfs, List<string> senhas, string cpf = "")
        {
            int ctlTentativas = 1;
            if (cpf == "")
            {
                Console.WriteLine("   ----------------------------------");
                Console.WriteLine("            Login em Conta ");
                Console.WriteLine("   ----------------------------------");
                Console.Write("   Informe o CPF para acessar a Conta ou Digite '0' para retornar: ");
                cpf = Console.ReadLine();
            }
            int cpfCheck = BuscaCPF(cpfs, cpf);
            if (cpfCheck == -1 && cpf != "0")
            {
                Console.WriteLine("  Tente novamente");
                Console.WriteLine("   Conta não encontrada, tente novamente!");
                return false;
            }
            while (ctlTentativas <= 3 && cpf != "0")
            {
                Console.WriteLine("   ----------------------------------");
                Console.Write("   Informe a senha para continuar: ");
                string senhaCheck = Console.ReadLine();
                Console.WriteLine("   ----------------------------------");
                if (senhaCheck != senhas[cpfCheck] && ctlTentativas < 3)
                {
                    Console.WriteLine($"   Senha incorreta! Restam {3 - ctlTentativas} tentativas!");
                }
                else if (senhaCheck == senhas[cpfCheck])
                {
                    return true;
                }
                ctlTentativas += 1;
            }
            if (cpf != "0")
            {
                Console.WriteLine($"  Senha incorreta! Número de tentativas ESGOTADAS!");
            }
            return false;
        }
        static bool SacarValor(List<string> cpfs, List<string> senhas, List<double> saldos, string cpfSacar = "", double valorSaque = 0)
        {
            bool ctlMsg = false;
            if (cpfSacar == "")
            {
                Console.WriteLine("   ----------------------------------");
                Console.WriteLine("            Saque em Conta");
                Console.WriteLine("   ----------------------------------");
                Console.Write("   Informe o CPF para realizar o Saque: ");
                cpfSacar = Console.ReadLine();
                ctlMsg = true;
            }
            int indexCpfSacar = BuscaCPF(cpfs, cpfSacar);

            if (indexCpfSacar == -1)
            {
                Console.WriteLine("\n  Tente novamente");
                Console.WriteLine("   Conta não encontrada.\n");
            }
            else
            {
                if (valorSaque == 0d)
                {
                    Console.Write("Informe o Valor a ser Sacado: R$ ");
                    valorSaque = double.Parse(Console.ReadLine());
                }
                if (ValidaSenha(cpfs, senhas, cpfSacar))
                {
                    if (valorSaque > 0d && valorSaque <= saldos[indexCpfSacar])
                    {
                        saldos[indexCpfSacar] -= valorSaque;
                        if (ctlMsg == true)
                        {
                            Console.WriteLine("\n   Saque Realizado com Sucesso!\n");
                        }
                        return true;
                    }
                    else if (valorSaque <= 0)
                    {
                        Console.WriteLine("\n  Tente novamente");
                        Console.WriteLine();
                        Console.WriteLine("  Valor de Saque inválido!\n");
                    }
                    else
                    {
                        Console.WriteLine("\n  Tente novamente"); 
                        Console.WriteLine();
                        Console.WriteLine("  Saldo Insuficiente!\n");
                    }
                }
            }
            return false;
        }
        static void ShowMenuConta()
        {
            
            Console.WriteLine();
            Console.WriteLine("   ----------------------------------------");
            Console.WriteLine("            Bem vindo ao ByteBank");
            Console.WriteLine("   ----------------------------------------");
            Console.WriteLine("   1 - Depositar");
            Console.WriteLine("   2 - Transferir");
            Console.WriteLine("   3 - Sacar");
            Console.WriteLine("   0 - Sair e retornar ao menu principal");
            Console.WriteLine("   ----------------------------------------");
            Console.WriteLine();
            Console.Write("   Digite a opção desejada: ");
        }
        static void MenuConta(List<string> cpfs, List<string> titulares, List<string> senhas, List<double> saldos)
        {
            int option;
            do
            {
                ShowMenuConta();
                option = int.Parse(Console.ReadLine());

                switch (option)
                {
                    case 0:
                        Console.Clear();
                        break;
                    case 1:
                        Console.Clear();
                        DepositarValor(cpfs, senhas, saldos);
                        break;
                    case 2:
                        Console.Clear();
                        TransferirValor(cpfs, senhas, saldos);
                        break;
                    case 3:
                        Console.Clear();
                        SacarValor(cpfs, senhas, saldos);
                        break;
                    default:
                        Console.Clear();
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine("\n   Opção Inválida!! Selecione uma das opções abaixo!\n");
                        Console.ForegroundColor = ConsoleColor.White;
                        break;
                }
            } while (option != 0);
        }
        public static void Main(string[] args)
        {

            

            List<string> cpfs = new List<string>();
            List<string> titulares = new List<string>();
            List<string> senhas = new List<string>();
            List<double> saldos = new List<double>();

            int option;

            do
            {
                ShowMenu();
                option = int.Parse(Console.ReadLine());

                Console.WriteLine();
                Console.WriteLine("   -----------------");

                switch (option)
                {
                    case 0:
                        Console.Clear();
                        Console.WriteLine("   Estou encerrando o programa...");
                        break;
                    case 1:
                        Console.Clear();
                        RegistraUsuarios(cpfs, titulares, senhas, saldos);
                        break;
                    case 2:
                        Console.Clear();
                        DeletaUsuarios(cpfs, titulares, senhas, saldos);
                        break;
                    case 3:
                        Console.Clear();
                        ListarTodasAsContas(cpfs, titulares, saldos);
                        break;
                    case 4:
                        Console.Clear();
                        ApresentarUsuario(cpfs, titulares, saldos);
                        break;
                    case 5:
                        Console.Clear();
                        ApresentarValorAcumulado(saldos);
                        break;
                    case 6:
                        Console.Clear();
                        if (ValidaSenha(cpfs, senhas))
                        {
                            MenuConta(cpfs, titulares, senhas, saldos);
                        }
                        break;
                    default:
                        Console.Clear();
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine("\n   Opção Inválida!! Selecione uma das opções abaixo!\n");
                        Console.ForegroundColor = ConsoleColor.White;
                        break;
                }
                Console.WriteLine();
                

            } while (option != 0);



        }

    }

}