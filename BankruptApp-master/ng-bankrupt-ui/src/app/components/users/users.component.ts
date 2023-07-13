import { HttpErrorResponse, HttpEventType, HttpResponse } from '@angular/common/http';
import { Component, EventEmitter, Output } from '@angular/core';
import { MatTableDataSource } from '@angular/material/table';
import { UserDocument } from 'src/app/models/userDocument';
import { AuthService, USER_ID } from 'src/app/services/auth.service';
import { UserstoreService } from 'src/app/services/userstore.service';

@Component({
  selector: 'app-users',
  templateUrl: './users.component.html',
  styleUrls: ['./users.component.scss']
})
export class UsersComponent {

  @Output() public onUploadFinished = new EventEmitter();

  documentsDs: MatTableDataSource<UserDocument>; 
  documentsColumns = ['docName', 'docDate', 'docDelete'];
  currentUserId: string = "";
  header = "Ошибка";
  message = "";
  visible = false;

  constructor(private us: UserstoreService, private as: AuthService) 
  { 
    this.documentsDs = new MatTableDataSource();
  }

  ngOnInit(): void {
    this.updateDocuments();
  }

  updateDocuments(){
    let id = localStorage.getItem(USER_ID);
    if(!id) return;
    this.currentUserId = id;
      this.us.getUserDocuments(id).subscribe(
        res => 
        {
          this.documentsDs = new MatTableDataSource(res); 
          this.documentsDs.filterPredicate = (document: UserDocument, filter: string): boolean => {
            if (filter) {
              return (document.date.includes(filter) || document.fileName.includes(filter));
            } else {
              return true;
            }
          };     
        },
        (err: HttpErrorResponse) => this.handleError(err));
  }

  uploadFile(files: FileList | null) {
    if (!files || files.length === 0)
      return;
    let fileToUpload = <File>files[0];
    const formData = new FormData();
    formData.append('file', fileToUpload, fileToUpload.name);
    formData.append('userId', this.currentUserId);
    
    this.us.uploadDocuments(formData).subscribe(
        (event) => {
          if (event.type === HttpEventType.Response) {
            this.onUploadFinished.emit(event.body);
            this.updateDocuments();
          }
        },
        (err: HttpErrorResponse) => this.handleError(err)
    );
  }

  getUserDocument(document: UserDocument) {
    let id = localStorage.getItem(USER_ID);
    this.us.getDocument(document.id, <string>id).subscribe((event) => {
      if (event.type === HttpEventType.Response) {
        this.downloadFile(event, document.fileName);
      }
    },
    (err: HttpErrorResponse) => this.handleError(err));
  }

  deleteDocument(document: UserDocument) {
    if(confirm('Вы уверены, что хотите удалить документ "' +document.fileName+'"')) {
      let id = this.as.getCurrentUserId();
      if(!id) return;
      this.us.deleteDocument(document.id, id).subscribe(
        (event) => {
          if (event.type === HttpEventType.Response) {
            this.updateDocuments();
          }
        },
      (err: HttpErrorResponse) => this.handleError(err));
    }
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

  applyFilter(filter: any) {
    var value = filter.value;    
    this.documentsDs.filter = value.trim();
  }

  handleError(error: HttpErrorResponse): void {
    this.message = error.error.message;
    this.header = "Ошибка " + error.status;
    this.visible = !this.visible;
  }
}
