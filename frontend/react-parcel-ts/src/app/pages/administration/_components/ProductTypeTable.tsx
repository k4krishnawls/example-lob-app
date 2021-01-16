import React from 'react';
import { Link } from 'react-router-dom';
import { Response } from "../../../utilities/useFetch";
import { ProductType } from "../../../types";

interface IProps {
  productTypes: Response<ProductType[]>;
}

export const ProductTypeTable: React.FunctionComponent<IProps> = (props: IProps) => {
  const { productTypes } = props;
  return (
    <table className="admin-table">
      <thead>
        <tr>
          <th>Id</th>
          <th>Name</th>
          <th>Updated</th>
          <th></th>
        </tr>
      </thead>
      <tbody>
        {productTypes.isLoading || productTypes.data === null
          ? <tr><td colSpan={5}>Loading...</td></tr>
          : <>
            {productTypes.data!.map(pt => (
              <tr key={pt.id}>
                <td>{pt.id}</td>
                <td>{pt.displayName}</td>
                <td>{pt.updatedOn.toLocaleDateString("en-US", { year: 'numeric', month: 'short', day: 'numeric' })}</td>
                <td>
                  <Link to={`/administration/productTypes/${pt.id}`} className="gdb-button gdb-bs-primary">Edit</Link>
                </td>
              </tr>
            ))}
          </>
        }
      </tbody>
      <tfoot>
        <tr>
          <td></td>
          <td></td>
          <td></td>
          <td></td>
          <td>
            <Link to="/administration/productTypes/new" className="gdb-button gdb-bs-primary">Add New</Link>
          </td>
        </tr>
      </tfoot>
    </table>
  );
};
