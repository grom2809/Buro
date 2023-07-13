import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Inject, Injectable } from '@angular/core';
import { BehaviorSubject, Observable, Subject } from 'rxjs';
import { STORE_API_URL } from '../app-injection-tokens';
import { User } from '../models/user';
import { UserDocument } from '../models/userDocument';

@Injectable({
  providedIn: 'root'
})
export class UserstoreService {

  private baseUserApiUrl = this.apiUrl + "/User/";
  private baseDocumentApiUrl = this.apiUrl + "/Document/";
  public currentUser: User|null;

  // private userId = new BehaviorSubject('');
  // currentUserId  = this.userId.asObservable();
  // updateUserId(userId: string) {
  //   this.userId.next(userId);
  // }

  constructor(private http: HttpClient, @Inject(STORE_API_URL) private apiUrl: string) 
  { 
    this.currentUser = null;
  }

  getUsers(): Observable<User[]> {
    return this.http.get<User[]>(this.baseUserApiUrl + 'All');
  }

  getUser(id: string): Observable<User> {
    return this.http.get<User>(this.baseUserApiUrl + id);
  }

  createUser(name: string, birthdate: string, email: string, password: string) {
    var body = {name: name, birthdate: birthdate, email: email, password: password};
    return this.http.post(this.baseUserApiUrl + 'Register', JSON.stringify(body), 
    {headers : new HttpHeaders({ 'Content-Type': 'application/json'}), reportProgress: true, observe: 'events'})
  }
  
  updateUser(id: string, name: string, birthdate: string, email: string, password: string) {
    var body = {id: id, name: name, birthdate: birthdate, email: email, password: password};
    return this.http.post(this.baseUserApiUrl + 'Update', JSON.stringify(body), {headers : new HttpHeaders({ 'Content-Type': 'application/json'}), reportProgress: true, observe: 'events'})
  }

  deleteAccount(id: string) {
    return this.http.delete(this.baseUserApiUrl + id, {reportProgress: true, observe: 'events'})
  }

  getUserDocuments(userId: string): Observable<UserDocument[]> {
    if(userId != "")
      return this.http.get<UserDocument[]>(this.baseDocumentApiUrl + 'User/' + userId);
    return new Observable<UserDocument[]>();
  }

  getDocument(id: string, userId: string) {
    return this.http.get(this.baseDocumentApiUrl + 'id=' + id + '&userId=' + userId, { reportProgress: true, observe: 'events', responseType: 'blob'});
  }

  deleteDocument(id: string, userId: string) {
    return this.http.delete(this.baseDocumentApiUrl + 'id=' + id + '&userId=' + userId, {reportProgress: true, observe: 'events'})
  }

  uploadDocuments(formData: FormData) {
    return this.http.post(this.baseDocumentApiUrl + 'Upload', formData, {reportProgress: true, observe: 'events'})
  }
}
