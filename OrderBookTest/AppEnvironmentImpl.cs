using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Resources;
using System.Xml.Linq;
using CMCMarkets.Prophet.OrderBookTest.api;
using CMCMarkets.Prophet.OrderBookTest.Logging;
using Action = CMCMarkets.Prophet.OrderBookTest.api.Action;

namespace CMCMarkets.Prophet.OrderBookTest
{
   
    class AppEnvironmentImpl : IAppEnvironment
    {
        private readonly ILog log;
        public delegate void OrderActionEventHandler(object sender, OrderActionEventArgs e);
        public delegate void ProcessingStartEventHandler(object sender, ProcessingStartEventArgs e);

        public event OrderActionEventHandler OrderActionEvent;
        public event ProcessingStartEventHandler ProcessingStartEvent;
        public event EventHandler ProcessingFinishEvent;
        
        public AppEnvironmentImpl(LogLevel level)
        {
            log = new Logger();
          
        }

        

        public void Run()
        {
            OnProcessingStart(new ProcessingStartEventArgs(log));
            FeedOrders();
            OnProcessingFinish();
        }

        void FeedOrders()
        {

            var xDocument = XDocument.Load("Resources/orders2.xml");
            
            //Command[] commands = {
            //                         new Command(Action.ADD, 1L, "MSFT.L", true, 5, 200),
            //                            new Command(Action.ADD, 2L, "VOD.L", true, 15, 100),
            //                            new Command(Action.ADD, 3L, "MSFT.L", false, 5, 300),
            //                            new Command(Action.ADD, 4L, "MSFT.L", true, 7, 150),
            //                            new Command(Action.REMOVE, 1L, null, true, -1, -1),
            //                            new Command(Action.ADD, 5L, "VOD.L", false, 17, 300),
            //                            new Command(Action.ADD, 6L, "VOD.L", true, 12, 150),
            //                            new Command(Action.EDIT, 3L, null, true, 7, 200),
            //                            new Command(Action.ADD, 7L, "VOD.L", false, 16, 100),
            //                            new Command(Action.ADD, 8L, "VOD.L", false, 19, 100),
            //                            new Command(Action.ADD, 9L, "VOD.L", false, 21, 112),
            //                            new Command(Action.REMOVE, 5L, null, false, -1, -1) };

            var commands = GetCommands(xDocument.Descendants().Skip(1));

            foreach (Command command in commands)

            {
                OnOrderAction(new OrderActionEventArgs(command.Action, command.Order));
            }
        }

        private IEnumerable<Command> GetCommands(IEnumerable<XElement> xElements)
        {
            var commands = new List<Command>();

            foreach (var xElement in xElements)
            {
                Action action;
                if (!Enum.TryParse(xElement.Name.ToString().ToUpper(), out action)) continue;
                
                var orderId = long.Parse(xElement.Attribute("order-id").Value);
                if (action == Action.REMOVE)
                {
                    commands.Add(new Command(action, orderId, null, true, -1, -1));
                    continue;
                }

                var price = Convert.ToInt32(xElement.Attribute("price").Value);
                var quantity = Convert.ToInt32(xElement.Attribute("quantity").Value);
                if (action == Action.EDIT)
                {
                    commands.Add(new Command(action, orderId, null, true, price, quantity));
                    continue;
                }

                var symbol = xElement.Attribute("symbol").Value;
                var isBuy = xElement.Attribute("type").Value.ToLower().Equals("buy");
                commands.Add(new Command(action, orderId, symbol, isBuy, price, quantity));
            }

            return commands;
        }

        protected void OnProcessingStart(ProcessingStartEventArgs args)
        {
            ProcessingStartEvent(this, args);
        }

        protected void OnProcessingFinish()
        {
            ProcessingFinishEvent(this, null);
        }

        protected void OnOrderAction(OrderActionEventArgs args)
        {
            OrderActionEvent(this, args);
        }

        private class Command
        {
            public Action Action { get; private set; }
            public Order Order {get; private set; }
            
            public Command(Action action, long orderId, String symbol, bool isBuy, int price, int quantity)
            {
                this.Action = action;
                this.Order = new Order(orderId, symbol, isBuy, price, quantity);
            }
        }
    }
}
