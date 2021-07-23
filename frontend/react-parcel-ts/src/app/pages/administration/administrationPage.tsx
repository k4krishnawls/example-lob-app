import React from 'react';
import { Section } from "../../components/Section";
import { User } from "../../types";
import { useFetch } from "../../utilities/useFetch";
import { UserTable } from './_components/UserTable';

export const AdministrationPage: React.FunctionComponent = () => {
  const users = useFetch<User[]>("/api/fe/users", (rd: any) => {
    return rd.map((ru: any) => ({
      id: ru.id,
      username: ru.username,
      name: ru.name,
      userType: ru.userType,
      createdOn: new Date(ru.createdOn)
    }));
  });

  return (
    <>
      <Section>
        <h1>Users</h1>
        <UserTable users={users} />
      </Section>
    </>
  );
};
