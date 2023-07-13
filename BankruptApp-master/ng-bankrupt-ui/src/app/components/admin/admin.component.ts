import { HttpErrorResponse, HttpEventType, HttpResponse } from '@angular/common/http';
import { Component } from '@angular/core';
import { MatTableDataSource } from '@angular/material/table';
import { User } from 'src/app/models/user';
import { UserDocument } from 'src/app/models/userDocument';
import { USER_ID } from 'src/app/services/auth.service';
import { UserstoreService } from 'src/app/services/userstore.service';

@Component({
  selector: 'app-admin',
  templateUrl: './admin.component.html',
  styleUrls: ['./admin.component.scss']
})
export class AdminComponent {

  usersDs: MatTableDataSource<User>;
  documentsDs: MatTableDataSource<UserDocument>; 
  usersColumns = ['id', 'name', 'birthdate', 'email']
  documentsColumns = ['docName']
  selectedUserName: string = "";
  selectedUser: User = new User("","","","","");
  header = "Ошибка";
  message = "";
  visible = false;

  constructor(private us: UserstoreService) {
    this.usersDs = new MatTableDataSource();
    this.documentsDs = new MatTableDataSource();
  }
  
  getSelectedUserName(){
    return this.selectedUserName=="" ? "Документы пользователя" : 'Документы пользователя "' + this.selectedUserName + '"';
  }

  ngOnInit(): void {
    this.us.getUsers().subscribe(
      res => 
      {
        this.usersDs = new MatTableDataSource(res)
      },
      (err: HttpErrorResponse) => this.handleError(err));
  }

  applyFilter(filter: any) {
    var value = filter.value;
    if(value != null)
      this.usersDs.filter = value.trim().toLowerCase();
  }

  getUserDocuments(user: User) {
    this.documentsDs.data = [];
    this.selectedUser = user;
    this.us.getUserDocuments(user.id).subscribe(
      res => 
      {
        this.selectedUserName = user.name;
        if(res.length != 0)
        {
          this.documentsDs = new MatTableDataSource(res);
        }        
      },
      (err: HttpErrorResponse) => this.handleError(err));
    this.documentsDs.data = this.documentsDs.data;
  }

  getUserDocument(document: UserDocument) {
    let id = this.selectedUser.id;
    this.us.getDocument(document.id, id).subscribe((event) => {
      if (event.type === HttpEventType.Response) {
        this.downloadFile(event, document.fileName);
      }
    },
    (err: HttpErrorResponse) => this.handleError(err));
  }

  private downloadFile = (data: HttpResponse<Blob>, name: string) => {
    if(data.body != null)
    {
      const downloadedFile = new Blob([data.body], { type: data.body.type });
      const a = document.createElement('a');
      a.setAttribute('style', 'display:none;');
      document.body.appendChild(a);
      a.download = name;
      a.href = URL.createObjectURL(downloadedFile);
      a.target = '_blank';
      a.click();
      document.body.removeChild(a);
    }
  }

  handleError(error: HttpErrorResponse): void {
    this.message = error.error.message;
    this.header = "Ошибка " + error.status;
    this.visible = !this.visible;
  }
}
