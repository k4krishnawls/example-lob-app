import React from 'react';
import { Link } from 'react-router-dom';
import { Response } from "../../../utilities/useFetch";
import { User } from "../../../types";

interface IProps {
  users: Response<User[]>;
}

export const UserTable: React.FunctionComponent<IProps> = (props: IProps) => {
  const { users } = props;
  return (
    <table className="admin-table">
      <thead>
        <tr>
          <th>Id</th>
          <th>Username</th>
          <th>Name</th>
          <th>Type</th>
          <th></th>
        </tr>
      </thead>
      <tbody>
        {users.isLoading || users.data === null
          ? <tr><td colSpan={5}>Loading...</td></tr>
          : <>
            {users.data!.map(u => (
              <tr key={u.id}>
                <td>{u.id}</td>
                <td>{u.username}</td>
                <td>{u.name}</td>
                <td>{u.userType}</td>
                <td>
                  <Link to={`/administration/users/${u.id}`} className="lob-button lob-bs-primary">Edit</Link>
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
            <Link to="/administration/users/new" className="lob-button lob-bs-primary">Add New</Link>
          </td>
        </tr>
      </tfoot>
    </table>
  );
};
