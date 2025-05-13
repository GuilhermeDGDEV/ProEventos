import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { map, Observable, ReplaySubject, take } from 'rxjs';
import { environment } from '@environments/environment';
import { User } from '@app/models/identity/User';
import { UserUpdate } from '@app/models/identity/UserUpdate';

@Injectable()
export class AccountService {

  private currentUserSource = new ReplaySubject<User | null>(1);
  public currentUser$ = this.currentUserSource.asObservable();

  public baseURL: string = environment.apiURL + 'api/account/';

  constructor(private http: HttpClient) { }

  public login(model: any): Observable<void> {
    return this.http.post<User>(this.baseURL + 'login', model)
      .pipe(take(1), map((response: User) => {
        const user = response;
        if (user) {
          this.setCurrentUser(user);
        }
      }));
  }

  public register(model: User): Observable<void> {
    return this.http.post<User>(this.baseURL + 'register', model)
      .pipe(take(1), map((response: User) => {
        const user = response;
        if (user) {
          this.setCurrentUser(user);
        }
      }));
  }

  public logout(): void {
    localStorage.removeItem('user');
    this.currentUserSource.next(null);
  }

  public setCurrentUser(user: User): void {
    if (user !== null) {
      localStorage.setItem('user', JSON.stringify(user));
      this.currentUserSource.next(user);
    }
  }

  public getUser(): Observable<UserUpdate> {
    return this.http.get<UserUpdate>(this.baseURL + 'getUser').pipe(take(1));
  }

  public updateUser(model: UserUpdate): Observable<void> {
    return this.http.put<UserUpdate>(this.baseURL + 'updateUser', model)
      .pipe(take(1), map((user: UserUpdate) => this.setCurrentUser(user)));
  }

}
