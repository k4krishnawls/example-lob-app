import React from 'react';
import { Link } from 'react-router-dom';
import { Section } from "../../components/section";
import { User } from "../../types";
import { useFetch } from "../../utilities/useFetch";

export const AdministrationPage: React.SFC = () => {
  const users = useFetch<User[]>("/api/fe/users", (rd: any) => {
    return rd.map((ru: any) => ({
      id: ru.id,
      username: ru.username,
      name: ru.user,
      userType: ru.userType,
      createdOn: new Date(ru.createdOn)
    }));
  });

  return (
    <Section>
      <h1>Users</h1>
      <table>
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
          {users.data == null
            ? <tr><td colSpan={5}>Loading...</td></tr>
            : <>
              {users.data.map(u => (
                <tr key={u.id}>
                  <td>{u.id}</td>
                  <td>{u.username}</td>
                  <td>{u.name}</td>
                  <td>{u.userType}</td>
                  <td>
                    {u.id <= 0
                      ? null
                      : <Link to={`/administration/users/${u.id}`}>Edit</Link>
                    }
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
              <Link to="/administration/users/new">Add New</Link>
            </td>
          </tr>
        </tfoot>
      </table>
    </Section>
  );
};
