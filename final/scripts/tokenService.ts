export default class TokenService {
  private tokenKey: string = 'token';
  private guidKey: string = 'guid';

  public setToken(token: string | undefined) {
    if (token === undefined) {
      localStorage.removeItem(this.tokenKey);
    } else {
      localStorage.setItem(this.tokenKey, token);
    }
  }

  public getToken(): string {
    return localStorage.getItem(this.tokenKey) ?? '';
  }

  public isLoggedIn(): boolean {
    // Won't work if the token is expired
    const token = this.getToken();
    return token !== '' && !(token.localeCompare('undefined') === 0);
  }

  public setGuid(guid: string | undefined | null) {
    if (guid === undefined) {
      const token = this.getToken();
      const guid = JSON.parse(atob(token.split('.')[1])).userId;
      localStorage.setItem(this.guidKey, guid);
    } else if (guid === null) {
      localStorage.removeItem(this.guidKey);
    } else {
      localStorage.setItem(this.guidKey, guid);
    }
  }

  public getGuid(): string {
    return localStorage.getItem(this.guidKey) ?? '';
  }

  public getUserName() {
    const token = this.getToken();
    if (token === '' || token === 'undefined') {
      return '';
    }
    console.log(JSON.parse(atob(token.split('.')[1])));
    return JSON.parse(atob(token.split('.')[1])).userName;
  }

  public generateTokenHeader() {
    return { Authorization: `Bearer ${this.getToken()}` };
  }
}
