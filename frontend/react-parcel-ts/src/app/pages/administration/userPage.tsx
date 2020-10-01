import React from 'react';
// import { Link } from 'react-router-dom';
import { Section } from "../../components/section";
import {
  User,
  Loadable,
  newLoadable,
  updateLoadable,
  LoadStatus,
  UserType
} from "../../types";

interface IProps {
  match: { params: { id: "new" | number } };
}

interface IState {
  user: Loadable<User>;
}

export class UserPage extends React.Component<IProps, IState>{
  constructor(props: IProps) {
    super(props);

    this.state = {
      user: newLoadable()
    };
  }

  async componentDidMount() {
    const { id } = this.props.match.params;
    if (id === "new") {
      this.initializeNewUser();
    }
    else {
      await this.loadUser(id);
    }
  }

  initializeNewUser() {
    this.setState({
      user: updateLoadable({
        id: 0,
        username: '',
        name: 'new user',
        userType: UserType.InteractiveUser,
        createdOn: new Date()
      }, LoadStatus.Loaded)
    });
  }

  async loadUser(id: number) {
    this.setState({ user: updateLoadable(null, LoadStatus.Loading) });
    const response = await fetch(`/api/fe/users/${id}`);
    if (response.ok) {
      const json = await response.json();
      const user = {
        id: json.id,
        username: json.username,
        name: json.user,
        userType: json.userType,
        createdOn: new Date(json.createdOn)
      };
      this.setState({ user: updateLoadable(user, LoadStatus.Loaded) });
    }
    else {
      this.setState({ user: updateLoadable(null, LoadStatus.Error) });
    }
  }

  public render() {
    const { status } = this.state.user;
    if (status == LoadStatus.New || status == LoadStatus.Loading) {
      return <Section>Loading...</Section>;
    }
    else if (status == LoadStatus.Error) {
      return <Section>An error occurred.</Section>;
    }
    else {
      const user = this.state.user.data!;
      return (
        <Section>
          <h1>User: {user.name}</h1>
          <label >
            <span>Name:</span>
            <input type="text" value={user.name} onChange={(v) => this.updateUser(v.target.value)} />
          </label>
        </Section>
      );
    }
  }

  public updateUser(change: any) {
    const user = {
      ...this.state.user,
      ...change
    };
    this.setState({ user: updateLoadable(user, LoadStatus.Loaded) });
  }
}
