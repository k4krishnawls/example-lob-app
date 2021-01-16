
export interface Loadable<T> {
  data: T | null;
  status: LoadStatus;
}

export const newLoadable = <T>() => ({
  data: null,
  status: LoadStatus.New
});

export const updateLoadable = <T>(data: T | null, status: LoadStatus) => ({
  data,
  status
});

export enum LoadStatus {
  New,
  Loading,
  Loaded,
  Error
}

export interface User {
  id: number;
  username: string;
  name: string;
  userType: UserType;
  createdOn: Date;
}

export enum UserType {
  InteractiveUser = 1,
  SystemUser = 2
}

export interface ProductType {
  id: number;
  displayName: string;
  updatedOn: Date;
}
