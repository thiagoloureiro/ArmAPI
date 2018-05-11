using RabbitMQ.Client;
using System;
using System.IO;
using System.Text;

namespace ArmAPI.Rabbit
{
    public class MessageBroker
    {
        private ConnectionFactory factory;
        private IConnection connection;
        private IModel channel;
        public string QueueName { get; set; }

        public void Connect()
        {
            factory = new ConnectionFactory() { HostName = "hound.rmq.cloudamqp.com", VirtualHost = "usldnewk", UserName = "usldnewk", Password = "urWrV6UeRu5vGgk8k7YJaqVwFCJyPSHc" };

            connection = factory.CreateConnection();
            // connection.ConnectionShutdown += Connection_ConnectionShutdown;

            channel = connection.CreateModel();
            channel.QueueDeclare(QueueName, true, false, false, null);
        }

        //private void Connection_ConnectionShutdown(object sender, ShutdownEventArgs e)
        //{
        //    Console.WriteLine("Connection broke!");

        //    Cleanup();

        //    while (true)
        //    {
        //        try
        //        {
        //            Connect();

        //            Console.WriteLine("Reconnected!");
        //            break;
        //        }
        //        catch (Exception ex)
        //        {
        //            Console.WriteLine($"Reconnect failed! {ex.Message}");
        //            Thread.Sleep(3000);
        //        }
        //    }
        //}

        private void Cleanup()
        {
            try
            {
                if (channel != null && channel.IsOpen)
                {
                    channel.Close();
                    channel = null;
                }

                if (connection != null && connection.IsOpen)
                {
                    connection.Close();
                    connection = null;
                }
            }
            catch (IOException ex)
            {
                Console.WriteLine($"Cleanup failed! {ex.Message}");

                // Close() may throw an IOException if connection
                // dies - but that's ok (handled by reconnect)
            }
        }

        public bool WriteMessageOnQueue(string queueName, string message)
        {
            try
            {
                QueueName = queueName;
                Connect();
                channel.BasicPublish(string.Empty, queueName, null, Encoding.ASCII.GetBytes(message));
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return false;
            }
            finally
            {
                Cleanup();
            }

            return true;
        }
    }
}