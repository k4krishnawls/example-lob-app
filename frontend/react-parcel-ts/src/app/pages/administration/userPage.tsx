import React from 'react';
import { Link } from 'react-router-dom';
import { AnimateOnChange } from 'react-animation';
import { Button } from '~app/components/button';
import { ButtonGroup } from '~app/components/ButtonGroup';
import { BadRequestType, parseBadRequestResponse } from '~app/utilities/badRequest';
import { Section } from "../../components/Section";
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
  id: "new" | number;
  user: Loadable<User>;
  saveStatus: "none" | "saving" | "saved";
  saveCompleteTimeout?: any;
  saveError?: string;
  saveFieldErrors?: { field: string, errors: string[] }[];
}

export class UserPage extends React.Component<IProps, IState>{
  constructor(props: IProps) {
    super(props);

    this.state = {
      id: this.props.match.params.id,
      user: newLoadable(),
      saveStatus: "none"
    };
  }

  async componentDidMount() {
    const { id } = this.state;
    if (id === "new") {
      this.initializeNewUser();
    }
    else {
      await this.loadUser(id);
    }
  }

  async componentDidUpdate(_: IProps, prevState: IState) {
    if (prevState.id !== this.state.id && this.state.id != "new") {
      await this.loadUser(this.state.id);
    }
  }

  initializeNewUser() {
    this.setState({
      id: "new",
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
        name: json.name,
        userType: json.userType,
        createdOn: new Date(json.createdOn)
      };
      this.setState({ user: updateLoadable(user, LoadStatus.Loaded) });
    }
    else {
      this.setState({ user: updateLoadable(null, LoadStatus.Error) });
    }
  }

  async saveUser() {
    const { user, saveCompleteTimeout } = this.state;
    if (!user.data) return;

    if (saveCompleteTimeout) clearTimeout(saveCompleteTimeout);
    this.setState({ saveStatus: "saving", saveError: undefined, saveFieldErrors: undefined, saveCompleteTimeout: undefined });

    if (this.state.id == "new") {
      const response = await fetch(`/api/fe/users`, {
        method: "POST",
        body: JSON.stringify({
          name: user.data.name,
          username: user.data.username
        }),
        headers: {
          'Content-Type': 'application/json;charset=utf-8'
        },
      });
      if (response.ok) {
        const json = await response.json();
        this.setState({
          saveStatus: "saved",
          id: json.id,
          saveCompleteTimeout: setTimeout(() => this.saveComplete(), 2000)
        });
      }
      else {
        await this.processSaveErrors(response);
      }
    }
    else {
      const response = await fetch(`/api/fe/users/${this.state.id}`, {
        method: "POST",
        body: JSON.stringify({
          name: user.data.name,
          username: user.data.username
        }),
        headers: {
          'Content-Type': 'application/json;charset=utf-8'
        },
      });
      if (response.ok) {
        this.setState({
          saveStatus: "saved",
          saveCompleteTimeout: setTimeout(() => this.saveComplete(), 2000)
        });
      }
      else {
        await this.processSaveErrors(response);
      }
    }
  }

  saveComplete() {
    if (this.state.saveStatus === "saved") {
      this.setState({
        saveStatus: "none"
      });
    }
  }

  async processSaveErrors(response: Response) {
    if (response.status == 400) {
      const error = await parseBadRequestResponse(response);
      if (error.errorType == BadRequestType.GeneralError) {
        this.setState({ saveError: error.generalError, saveStatus: "none" });
      }
      else {
        this.setState({ saveError: error.generalError, saveFieldErrors: error.fieldErrors, saveStatus: "none" });
      }
    }
    else {
      this.setState({ saveError: "An error occurred while saving, please try again or contact support if it continues." });
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
          <h1>User: {user.name}{this.renderSaveStatus()}</h1>
          {this.renderErrors()}
          <label>
            <span>Name:</span>
            <input type="text" value={user.name} onChange={(v) => this.updateUser({ name: v.target.value })} />
          </label>
          <label>
            <span>Username:</span>
            <input type="text" value={user.username} onChange={(v) => this.updateUser({ username: v.target.value })} />
          </label>
          <ButtonGroup>
            <Link to="/administration" className="lob-button lob-bs-secondary">Cancel</Link>
            <Button
              enable={user.name.length > 0}
              onClick={() => this.saveUser()}
            >Save</Button>
          </ButtonGroup>
        </Section>
      );
    }
  }

  private renderSaveStatus() {
    switch (this.state.saveStatus) {
      case "saving":
        return <div className="lob-save-status">Saving...</div>;
      case "saved":
        return <AnimateOnChange><div className="lob-save-status">Saved</div></AnimateOnChange>;
      case "none":
      default:
        return null;
    }
  }

  private renderErrors() {
    const { saveError, saveFieldErrors } = this.state;
    if (!saveError && !saveFieldErrors) return;
    return (
      <div className="lob-form-error">
        {!saveError ? null :
          <div className="lob-form-error-message">{saveError}</div>
        }
        {!saveFieldErrors ? null :
          <dl className="lob-form-error-fields">
            {saveFieldErrors.map(f => (
              <>
                <dt>{f.field}</dt>
                <dd>{f.errors.join(", ")}</dd>
              </>
            ))}
          </dl>
        }
      </div>
    );
  }

  public updateUser(change: any) {
    const user = {
      ...this.state.user.data,
      ...change
    };
    this.setState({ user: updateLoadable(user, LoadStatus.Loaded) });
  }
}
