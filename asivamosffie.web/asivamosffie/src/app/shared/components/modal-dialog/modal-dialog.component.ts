import {Component, Inject} from '@angular/core';
import { MAT_DIALOG_DATA } from '@angular/material/dialog';
import { DomSanitizer } from '@angular/platform-browser';

@Component({
  selector: 'app-modal-dialog',
  templateUrl: './modal-dialog.component.html',
  styleUrls: ['./modal-dialog.component.scss']
})
export class ModalDialogComponent {

  constructor(@Inject(MAT_DIALOG_DATA) public data,private sanitized: DomSanitizer) {
    data.modalText=this.sanitized.bypassSecurityTrustHtml(data.modalText);
  }

}
