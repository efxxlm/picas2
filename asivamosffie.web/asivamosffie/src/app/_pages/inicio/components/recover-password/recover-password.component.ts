import { Component, OnInit } from '@angular/core';
import { FormControl, Validators } from '@angular/forms';
import { MatDialog } from '@angular/material/dialog';

@Component({
  selector: 'app-recover-password',
  templateUrl: './recover-password.component.html',
  styleUrls: ['./recover-password.component.scss']
})

export class RecoverPasswordComponent implements OnInit {

  emailRecoverField: FormControl;

  constructor(public dialog: MatDialog) {
    this.emailRecoverField =  new FormControl('', [
      Validators.required,
      Validators.maxLength(50),
      Validators.minLength(4),
      Validators.email,
      Validators.pattern(/^[a-zA-Z0-9._-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,6}$/)
    ]);
  }

  ngOnInit(): void {
  }

}
