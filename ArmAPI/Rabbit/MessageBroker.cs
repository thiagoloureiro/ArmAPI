using RabbitMQ.Client;
using System;
using System.IO;
using System.Text;

namespace ArmAPI.Rabbit
{
    public class MessageBroker
    {
        private ConnectionFactory _factory;
        private IConnection _connection;
        private IModel _channel;

        public string QueueName { get; set; }

        public void Connect()
        {
            _factory = new ConnectionFactory() { HostName = "hound.rmq.cloudamqp.com", VirtualHost = "usldnewk", UserName = "usldnewk", Password = "-" };

            _connection = _factory.CreateConnection();

            _channel = _connection.CreateModel();
            _channel.QueueDeclare(QueueName, true, false, false, null);
        }

        private void Cleanup()
        {
            try
            {
                if (_channel != null && _channel.IsOpen)
                {
                    _channel.Close();
                    _channel = null;
                }

                if (_connection != null && _connection.IsOpen)
                {
                    _connection.Close();
                    _connection = null;
                }
            }
            catch (IOException ex)
            {
                Console.WriteLine($"Cleanup failed! {ex.Message}");
            }
        }

        public bool WriteMessageOnQueue(string queueName, string message)
        {
            try
            {
                QueueName = queueName;
                Connect();
                _channel.BasicPublish(string.Empty, queueName, null, Encoding.ASCII.GetBytes(message));
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