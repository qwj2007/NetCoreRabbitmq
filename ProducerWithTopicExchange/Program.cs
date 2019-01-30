using System;
using System.Text;
using RabbitMQ.Client;

namespace ProducerWithTopicExchange
{
    class Program
    {
        static void Main(string[] args)
        {
			string exchangeName = "TestTopicChange";
	        string queueName = "hello";
	        string routeKey = "TestRouteKey.*";

	        //创建连接工厂
	        ConnectionFactory factory = new ConnectionFactory
	        {
		        UserName = "admin",//用户名
		        Password = "admin",//密码
		        HostName = "192.168.84.31"//rabbitmq ip
	        };

	        //创建连接
	        var connection = factory.CreateConnection();
	        //创建通道
	        var channel = connection.CreateModel();

	        //定义一个Direct类型交换机
	        channel.ExchangeDeclare(exchangeName, ExchangeType.Topic, false, false, null);

	        //定义队列1
	        channel.QueueDeclare(queueName, false, false, false, null);

	        //将队列绑定到交换机
	        channel.QueueBind(queueName, exchangeName, routeKey, null);



	        Console.WriteLine($"\nRabbitMQ连接成功，\n\n请输入消息，输入exit退出！");

	        string input;
	        do
	        {
		        input = Console.ReadLine();

		        var sendBytes = Encoding.UTF8.GetBytes(input);
	            //持久化
	            IBasicProperties properties = channel.CreateBasicProperties();
	            properties.DeliveryMode = 2;//
                //发布消息
                channel.BasicPublish(exchangeName, "TestRouteKey.one", null, sendBytes);

	        } while (input.Trim().ToLower() != "exit");
	        channel.Close();
	        connection.Close();
		}
    }
}
