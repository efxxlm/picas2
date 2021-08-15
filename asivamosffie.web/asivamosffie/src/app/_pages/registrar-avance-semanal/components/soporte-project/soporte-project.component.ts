import { Component, Inject, OnInit } from '@angular/core';
import { FormControl, Validators } from '@angular/forms';

@Component({
  selector: 'app-soporte-project',
  templateUrl: './soporte-project.component.html',
  styleUrls: ['./soporte-project.component.scss']
})
export class SoporteProjectComponent implements OnInit {

  archivo: string;

  documentFile: FormControl;

  constructor() { }

  ngOnInit(): void {
    this.declararDocumentFile();
  }

  private declararDocumentFile() {
    this.documentFile = new FormControl(null, [Validators.required]);
  }

  fileName(event: any) {
    if (event.target.files.length > 0) {
      const file = event.target.files[0];
      this.archivo = event.target.files[0].name;

    }
  }

}
