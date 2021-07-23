import React from 'react';
import { Link } from 'react-router-dom';
import { CurrencySpan } from '~app/components/inputs/CurrencySpan';
import { QuantitySpan } from '~app/components/inputs/QuantitySpan';
import { UserDateSpan } from '~app/components/inputs/UserDateSpan';
import { OrderSummary } from '~app/types';
import { Response } from '~app/utilities/useFetch';

interface IProps {
  orders: Response<OrderSummary[]>;
}

export const OrderSummaryList: React.FunctionComponent<IProps> = (props: IProps) => {
  const { orders } = props;

  if (orders.isLoading) {
    return (
      <span>Loading...</span>
    );
  }

  if (orders.error) {
    return (
      <span>Error: {orders.error}</span>
    );
  }

  if (!orders.data || orders.data.length == 0) {
    return (
      <span>No pending orders</span>
    );
  }

  return (
    <ul className="lob-comp-list">
      {orders.data!.map(o => (
        <li key={o.id}>
          <Link to={`/orders/${o.id}`}>
            <div>
              <img src={o.customer.avatar} alt="User avatar" aria-hidden />
              <div>
                <div><UserDateSpan date={o.orderDate} /></div>
                <div>by {o.customer.name}, <QuantitySpan number={o.itemCount} /> items</div>
              </div>
              <div><CurrencySpan number={o.orderTotal} /></div>
            </div>
          </Link>
        </li>
      ))}
    </ul>
  );
};
