using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using OpenNETCF.MQTT;
using uPLibrary.Networking.M2Mqtt;
using uPLibrary.Networking.M2Mqtt.Messages;

namespace mosk
{
    /// <summary>
    /// Logika interakcji dla klasy MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MqttClient.MqttMsgPublishEventHandler client_MqttMsgPublishReceived { get; private set; }

        //MQTTClient client;
        //string clientId;
        public MainWindow()
        {
            InitializeComponent();
            //string BrokerAddress = "91.1.46.197.135";
            //int BrokerPort = 10883;

            //client = new MQTTClient(BrokerAddress, BrokerPort);


            //// use a unique id as client id, each time we start the application
            //clientId = Guid.NewGuid().ToString();

            //client.Connect(clientId);
            //if (client.IsConnected) MessageBox.Show("Polaczono!");
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            OdbierzWiadomosc1();
        }

        void OdbierzWiadomosc1()
        {
            var IP = IPAddress.Parse("xxx.xxx.xxx.xxx");
            var port = Convert.ToInt32(10883);
            MqttClient client = new MqttClient(IP, port, false, null, null, MqttSslProtocols.None);
            client.ProtocolVersion = MqttProtocolVersion.Version_3_1;
            string[] sub = { "test" };
            byte[] qos = { MqttMsgBase.QOS_LEVEL_EXACTLY_ONCE };
            client.Subscribe(sub, qos);
            client.MqttMsgPublishReceived += client_recievedMsg;
            //    client.MqttMsgPublishReceived += new MqttClient.MqttMsgPublishEventHandler(EventPublished);
            //---------------
            client.Connect("Klient123a");
            client.Subscribe(new string[] { "test" }, new byte[]{2});
            
            //-----------------
            if (client.IsConnected) client.Disconnect();
        }
         void EventPublished(Object sender, uPLibrary.Networking.M2Mqtt.Messages.MqttMsgPublishEventArgs e)
        {
            MessageBox.Show("The Topic is:" + e.Topic);
            MessageBox.Show(e.Message.ToString());
        }

        void client_recievedMsg(object sender, MqttMsgPublishEventArgs e)
        {
            byte[] msg = e.Message;
            string topic = e.Topic.ToString();
            MessageBox.Show(topic+" "+msg);
            MessageBox.Show(Encoding.UTF8.GetString(e.Message));
        }

        void OdbierzWiadomosc()
        {
            var IP = IPAddress.Parse("xxx.xxx.xxx.xxx");
            var port = Convert.ToInt32(10883);
            string Topic = "test";
            MqttClient client = new MqttClient(IP,port, false, null, null, MqttSslProtocols.None);
           // byte code = client.Connect(Guid.NewGuid().ToString());
            client.ProtocolVersion = MqttProtocolVersion.Version_3_1;

            byte code = client.Connect("Klient123", null, null,
                           false, // will retain flag
                           MqttMsgBase.QOS_LEVEL_EXACTLY_ONCE, // will QoS
                           true, // will flag
                           "/will_topic", // will topic
                           "will_message", // will message
                           true,
                           60);
            client.Subscribe(new string[] { Topic }, new byte[] { MqttMsgBase.QOS_LEVEL_EXACTLY_ONCE });
            client.MqttMsgPublishReceived += client_MqttMsgPublishReceived;

            client.Disconnect();
        }
        void client_MqttMsgSubscribed(object sender, MqttMsgSubscribedEventArgs e)
        {
            MessageBox.Show("Subscribed for id = " + e.MessageId);
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            WyslijWiadomosc();
        }

        void WyslijWiadomosc()
        {
            var receivedMessage = false;
            string message = "";
            // create the client
            var client = new MQTTClient("xxx.xxx.xxx.xxx", 10883);

            // hook up the MessageReceived event with a handler
            //client.MessageReceived += (topic, qos, payload) =>
            //{
            //    MessageBox.Show("Odebrano: " + topic);
            //    receivedMessage = true;
            //};

            // connect to the MQTT server
            message = Guid.NewGuid().ToString();
            client.Connect("Klient1234");
            // wait for the connection to complete

            // add a subscription
            client.Subscriptions.Add(new Subscription("test"));

            string User = "";
            User = client.BrokerHostName;
            var data = DateTime.Now;
            client.Publish("test", data + " Wiadomosc: " + message, QoS.AcknowledgeDelivery, false);
            status.Content = "Wysłano" ;
            if (client.IsConnected) 
            client.Disconnect();
        }

    }
}
