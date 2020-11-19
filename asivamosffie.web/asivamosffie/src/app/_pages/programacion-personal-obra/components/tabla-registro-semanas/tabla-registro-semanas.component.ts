import { Component, EventEmitter, Input, OnInit, Output } from '@angular/core';
import { MatDialog } from '@angular/material/dialog';
import { MatTableDataSource } from '@angular/material/table';
import { Router } from '@angular/router';
import { ProgramacionPersonalObraService } from 'src/app/core/_services/programacionPersonalObra/programacion-personal-obra.service';
import { ModalDialogComponent } from 'src/app/shared/components/modal-dialog/modal-dialog.component';
import { DetalleProgramacionPersonal, pContratoConstruccion } from 'src/app/_interfaces/programacionPersonal.interface';

@Component({
  selector: 'app-tabla-registro-semanas',
  templateUrl: './tabla-registro-semanas.component.html',
  styleUrls: ['./tabla-registro-semanas.component.scss']
})
export class TablaRegistroSemanasComponent implements OnInit {

  @Input() registroSemanas: DetalleProgramacionPersonal[];
  @Input() contratoConstruccionId: number;
  @Output() seRealizoPeticion = new EventEmitter<boolean>();
  registroSemanasTabla: any[] = [];

  constructor ( private dialog: MatDialog,
                private programacionPersonalSvc: ProgramacionPersonalObraService,
                private routes: Router )
  {
  };

  ngOnInit(): void {
    if ( this.registroSemanas !== undefined ) {
      let numeroregistros = 0;
      this.registroSemanasTabla.push( [] );
      this.registroSemanas.forEach( registro => {
        if ( this.registroSemanasTabla[ numeroregistros ].length < 20 ) {
          registro[ 'cantidadPersonal' ] = registro[ 'cantidadPersonal' ] !== undefined ? String( registro[ 'cantidadPersonal' ] ) : null;
          this.registroSemanasTabla[ numeroregistros ].push( [ registro ] );
        };
        if ( this.registroSemanasTabla[ numeroregistros ].length >= 20 ) {
          this.registroSemanasTabla.push( [] );
          numeroregistros++;
        };
      } );
      for ( let registro of this.registroSemanasTabla ) {
        if ( registro.length < 20 ) {
          const bucleLimite = 20 - registro.length
          for( let i=0; i<bucleLimite; i++ ) {
            registro.push( [] );
          };
        };
      };
      console.log( this.registroSemanasTabla );
    }
  };

  openDialog (modalTitle: string, modalText: string) {
    this.dialog.open(ModalDialogComponent, {
      width: '28em',
      data : { modalTitle, modalText }
    });
  };

  guardarRegistros () {
    const pContratoConstruccion: pContratoConstruccion = {
      contratoConstruccionId: this.contratoConstruccionId,
      programacionPersonalContratoConstruccion: []
    };

    for( let registroSemanas of this.registroSemanasTabla ) {
      registroSemanas.forEach( registro => {
        if ( registro.length > 0 ) {
          pContratoConstruccion.programacionPersonalContratoConstruccion.push(
            {
              cantidadPersonal: registro[0].cantidadPersonal !== null && registro[0].cantidadPersonal.length > 0 ? Number( registro[0].cantidadPersonal ) : null,
              programacionPersonalContratoConstruccionId: registro[0].programacionPersonalContratoConstruccionId
            }
          );
        };
      });
    };

    console.log( pContratoConstruccion );
    this.programacionPersonalSvc.updateProgramacionContratoPersonal( pContratoConstruccion )
      .subscribe(
        response => {
          this.openDialog( '', response.message );
          this.seRealizoPeticion.emit( true );
        },
        err => this.openDialog( '', err.message )
      );

  };

};