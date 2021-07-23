export interface Loadable<T> {
  data: T | null;
  status: LoadStatus;
}

export const newLoadable = <T>() => ({
  data: null as T | null,
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



export type DailyRevenue = {
  date: Date;
  amount: number;
};

export enum OrderStatus {
  Ordered = 1,
  Processing = 2,
  Delivering = 3,
  Delivered = 4,
  Cancelled = 5
}

export type OrderSummary = {
  id: number;
  orderDate: Date;
  status: OrderStatus;
  customer: {
    id: number;
    name: string;
    avatar?: string;
  };
  itemCount: number;
  orderTotal: number;
};

export enum ReviewStatus {
  Pending = 1,
  Accepted = 2,
  Rejected = 3
}

export type ReviewSummary = {
  reviewDate: Date;
  score: number;
  excerpt: string;
  status: ReviewStatus;
  customer: {
    id: number;
    name: string;
    avatar?: string;
  };
};

