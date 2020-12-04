import { Component, Inject, OnInit } from '@angular/core';
import { MatDialogRef, MAT_DIALOG_DATA } from '@angular/material/dialog';
import { Router } from '@angular/router';
import { ProgramacionPersonalObraService } from 'src/app/core/_services/programacionPersonalObra/programacion-personal-obra.service';
import { DetalleProgramacionPersonal } from 'src/app/_interfaces/programacionPersonal.interface';

@Component({
  selector: 'app-dialog-registro-programacion',
  templateUrl: './dialog-registro-programacion.component.html',
  styleUrls: ['./dialog-registro-programacion.component.scss']
})
export class DialogRegistroProgramacionComponent implements OnInit {

  registroSemanas: DetalleProgramacionPersonal[];

  constructor ( private programacionPersonalSvc: ProgramacionPersonalObraService,
                private routes: Router,
                private dialogRef: MatDialogRef<DialogRegistroProgramacionComponent>,
                @Inject(MAT_DIALOG_DATA) public dataContrato )
  {
    this.programacionPersonalSvc.getProgramacionPersonalByContratoConstruccionId( dataContrato.contrato.contratoConstruccionId )
      .subscribe(
        response => {
          this.registroSemanas = response;
          console.log( this.registroSemanas );
        }
      );
  };

  ngOnInit(): void {
  };

  seRealizoPeticion ( peticion: boolean ) {
    if ( peticion === true ) {
      this.dialogRef.close( true );
    };
  }

};