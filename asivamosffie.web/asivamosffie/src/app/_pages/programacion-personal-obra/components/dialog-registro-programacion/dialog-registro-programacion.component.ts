import { Component, Inject, OnInit } from '@angular/core';
import { MAT_DIALOG_DATA } from '@angular/material/dialog';
import { ProgramacionPersonalObraService } from 'src/app/core/_services/programacionPersonalObra/programacion-personal-obra.service';

@Component({
  selector: 'app-dialog-registro-programacion',
  templateUrl: './dialog-registro-programacion.component.html',
  styleUrls: ['./dialog-registro-programacion.component.scss']
})
export class DialogRegistroProgramacionComponent implements OnInit {

  registroSemanas: any[];

  constructor ( private programacionPersonalSvc: ProgramacionPersonalObraService,
                @Inject(MAT_DIALOG_DATA) public dataContrato )
  {
    this.programacionPersonalSvc.getProgramacionPersonalByContratoConstruccionId( dataContrato.contrato.contratoConstruccionId )
      .subscribe(
        response => this.registroSemanas = response
      );
  };

  ngOnInit(): void {
  };

};