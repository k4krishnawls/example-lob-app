import React from 'react';
import { Section } from "../../components/section";
import { ProductType, User } from "../../types";
import { useFetch } from "../../utilities/useFetch";
import { ProductTypeTable } from './_components/ProductTypeTable';
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

  const productTypes = useFetch<ProductType[]>("/api/fe/products/types", (rd: any) => {
    return rd.map((ru: any) => ({
      id: ru.id,
      displayName: ru.displayName,
      updatedOn: new Date(ru.updatedOn)
    }));
  });

  return (
    <>
      <Section>
        <h1>Users</h1>
        <UserTable users={users} />
      </Section>
      <Section>
        <h1>ProductTypes</h1>
        <ProductTypeTable productTypes={productTypes} />
      </Section>
    </>
  );
};
