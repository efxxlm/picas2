import { Component, OnInit } from '@angular/core';
import { Validators, FormGroup, FormBuilder, FormControl } from '@angular/forms';
import { MatDialog } from '@angular/material/dialog';

@Component({
  selector: 'app-recover-password',
  templateUrl: './recover-password.component.html',
  styleUrls: ['./recover-password.component.scss']
})

export class RecoverPasswordComponent implements OnInit {

  formRecoverPass: FormGroup;

  constructor(
    private formBuilderRecoverPass: FormBuilder,
    public dialog: MatDialog
  ) {
    this.builderRecoverPass();
  }

  ngOnInit(): void {
  }

  RecoverPassword(event: Event) {
    event.preventDefault();
    if (this.formRecoverPass.valid) {
      console.log(this.formRecoverPass.value);
    }

  }

  private builderRecoverPass() {
    this.formRecoverPass = this.formBuilderRecoverPass.group({
      emailRecoverField: ['', [
          Validators.required,
          Validators.maxLength(50),
          Validators.minLength(4),
          Validators.email,
          Validators.pattern(/^[a-zA-Z0-9._-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,6}$/)
        ]]
    });
  }

}
