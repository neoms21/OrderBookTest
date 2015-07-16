namespace CMCMarkets.Prophet.OrderBookTest.api
{
    using System;

    interface IOrderConsumer
    {
        /// <summary>
        /// Called by the environment before any events are processed
        /// </summary>
        /// <param name="log">Logger to be used during processing</param>
        void StartProcessing(object sender, ProcessingStartEventArgs args);
        
        /// <summary>
        ///  Handles specific event with order data. The meaningful properties of the order depends on the
        ///  action. Note that the rest properties are with unspecified, but in usual cases invalid
        ///  values. meaningful props are:
        ///  <ul>
        ///     <li>For REMOVE: orderId</li>
        ///     <li>For EDIT: orderId, quantity and price</li>
        ///     <li>For ADD: orderId, symbol, isBuy, quantity and price</li>
        ///  </ul>
        /// </summary>
        /// <param name="action">The type of action</param>
        /// <param name="order">The order DTO</param>
        void HandleOrderAction(object sender, OrderActionEventArgs args);
        
        
        /// <summary>
        /// Called by the environment when no more events will be processed.
        /// </summary>
        void FinishProcessing(object sender, EventArgs args);
    }
}
