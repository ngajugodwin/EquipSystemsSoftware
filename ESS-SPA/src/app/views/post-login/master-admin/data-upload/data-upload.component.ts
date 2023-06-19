import { Component, OnInit } from '@angular/core';
import { NgxModalComponent } from 'ngx-modalview';
import { ModalData } from 'src/app/entities/models/modalData';

@Component({
  selector: 'app-data-upload',
  templateUrl: './data-upload.component.html',
  styleUrls: ['./data-upload.component.css']
})
export class DataUploadComponent extends NgxModalComponent<ModalData, any> implements OnInit, ModalData {
  selectedFile: any;
  
  title: string;
  message: string;
  data: any;

  constructor() { 
    super();
  }

  ngOnInit() {
  }

  onFileChanged(data: any) {
    this.selectedFile = data.target.files[0]
  }

  save() {
    if (this.selectedFile === null || this.selectedFile === undefined) {
      alert('Please select a file');
      return;
    }

    if (!this.selectedFile.name.includes('.jpg', 0)) {
      alert('Only jpg pictures are supported');
      return;
    }

    this.result = this.selectedFile;
    this.close();
  }

}
