import { Component, EventEmitter, Input, OnInit, Output } from '@angular/core';
import { MatDialog } from '@angular/material/dialog';
import { MatTableDataSource } from '@angular/material/table';
import { Router } from '@angular/router';
import { ProgramacionPersonalObraService } from 'src/app/core/_services/programacionPersonalObra/programacion-personal-obra.service';
import { ModalDialogComponent } from 'src/app/shared/components/modal-dialog/modal-dialog.component';
import { DetalleProgramacionPersonal, ContratoConstruccion } from 'src/app/_interfaces/programacionPersonal.interface';

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

  constructor(
    private dialog: MatDialog,
    private programacionPersonalSvc: ProgramacionPersonalObraService,
    private routes: Router )
  {
  }

  ngOnInit(): void {
    if ( this.registroSemanas !== undefined ) {
      let numeroregistros = 0;
      this.registroSemanasTabla.push( [] );
      console.log( this.registroSemanas );
      this.registroSemanas.forEach( registro => {
        if ( registro.seguimientoSemanalPersonalObra.length === 0 ) {
          registro.seguimientoSemanalPersonalObra.push(
            {
              cantidadPersonal: null
            }
          );
        }
        if ( this.registroSemanasTabla[ numeroregistros ].length < 20 ) {
          registro.cantidadPersonal = registro.cantidadPersonal !== undefined ? String( registro.cantidadPersonal ) : null;
          this.registroSemanasTabla[ numeroregistros ].push( [ registro ] );
        }
        if ( this.registroSemanasTabla[ numeroregistros ].length >= 20 ) {
          this.registroSemanasTabla.push( [] );
          numeroregistros++;
        }
      } );
      for ( const registro of this.registroSemanasTabla ) {
        if ( registro.length < 20 ) {
          const bucleLimite = 20 - registro.length;
          for ( let i = 0; i < bucleLimite; i++ ) {
            registro.push( [] );
          }
        }
      }
      console.log( this.registroSemanasTabla );
    }
  }

  openDialog(modalTitle: string, modalText: string) {
    this.dialog.open(ModalDialogComponent, {
      width: '28em',
      data : { modalTitle, modalText }
    });
  }

  guardarRegistros() {
    const contratoConstruccion: any[] = [];

    for ( const registroSemanas of this.registroSemanasTabla ) {
      registroSemanas.forEach( registro => {
        if ( registro.length > 0 ) {
          contratoConstruccion.push(
            {
              seguimientoSemanalId: registro[0].seguimientoSemanalId,
              contratacionProyectoId: registro[0].contratacionProyectoId,
              seguimientoSemanalPersonalObra: [
                {
                  seguimientoSemanalId: registro[0].seguimientoSemanalId,
                  cantidadPersonal: registro[0].seguimientoSemanalPersonalObra[0].cantidadPersonal !== null
                                    && registro[0].seguimientoSemanalPersonalObra[0].cantidadPersonal.length > 0 ?
                                    Number( registro[0].seguimientoSemanalPersonalObra[0].cantidadPersonal ) : null
                }
              ]
            }
          );
        }
      });
    }

    console.log( contratoConstruccion );
    this.programacionPersonalSvc.updateProgramacionContratoPersonal( contratoConstruccion )
      .subscribe(
        response => {
          this.openDialog( '', response.message );
          this.seRealizoPeticion.emit( true );
        },
        err => this.openDialog( '', err.message )
      );

  }

}
