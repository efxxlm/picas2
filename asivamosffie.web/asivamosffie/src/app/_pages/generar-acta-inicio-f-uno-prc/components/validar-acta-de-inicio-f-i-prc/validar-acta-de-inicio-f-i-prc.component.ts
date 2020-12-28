import { Component, OnInit } from '@angular/core';
import { FormBuilder, Validators } from '@angular/forms';
import { MatDialog } from '@angular/material/dialog';
import { Router } from '@angular/router';
import { ModalDialogComponent } from 'src/app/shared/components/modal-dialog/modal-dialog.component';

@Component({
  selector: 'app-validar-acta-de-inicio-f-i-prc',
  templateUrl: './validar-acta-de-inicio-f-i-prc.component.html',
  styleUrls: ['./validar-acta-de-inicio-f-i-prc.component.scss']
})
export class ValidarActaDeInicioFIPreconstruccionComponent implements OnInit {
  
  dataDialog: {
    modalTitle: string,
    modalText: string
  };

  constructor(private router: Router,public dialog: MatDialog, private fb: FormBuilder) { }

  ngOnInit(): void {
    
  }

}
