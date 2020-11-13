import { Component, Input, OnInit, Output, EventEmitter } from '@angular/core';
import { DialogCargarProgramacionComponent } from '../dialog-cargar-programacion/dialog-cargar-programacion.component';
import { MatDialog } from '@angular/material/dialog';

@Component({
  selector: 'app-programacion-obra-flujo-inversion',
  templateUrl: './programacion-obra-flujo-inversion.component.html',
  styleUrls: ['./programacion-obra-flujo-inversion.component.scss']
})
export class ProgramacionObraFlujoInversionComponent implements OnInit {

  @Input() esFlujoInversion: boolean;
  @Input() contratoConstruccionId: number;
  @Input() observacionDevolucionProgramacionObra: number;
  @Output() terminoCarga = new EventEmitter();
  @Output() realizoObservacion = new EventEmitter();
  tieneRegistrosObra: boolean = true;
  tieneRegistrosInversion: boolean = true;

  constructor ( private dialog: MatDialog ) { }

  ngOnInit(): void {
  }

  cargarProgramacion () {
    const dialogCargarProgramacion = this.dialog.open( DialogCargarProgramacionComponent, {
      width: '75em',
      data: { esFlujoInversion: this.esFlujoInversion, contratoConstruccionId: this.contratoConstruccionId }
    });

    dialogCargarProgramacion.afterClosed().subscribe( response => {
      console.log( 'termino carga masiva?', response );
      this.terminoCarga.emit( response.terminoCarga );
    } );
  };

  esObservacion ( realizoObservacion: boolean ) {
    if ( realizoObservacion === true ) {
      this.realizoObservacion.emit( realizoObservacion );
    };
  };

}
