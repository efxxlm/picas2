import { Component, Input, OnInit, Output, EventEmitter } from '@angular/core';
import { DialogCargarProgramacionComponent } from '../dialog-cargar-programacion/dialog-cargar-programacion.component';
import { MatDialog } from '@angular/material/dialog';
import { FaseUnoConstruccionService } from 'src/app/core/_services/faseUnoConstruccion/fase-uno-construccion.service';
import { Contrato } from 'src/app/_interfaces/faseUnoPreconstruccion.interface';
import { Proyecto } from 'src/app/core/_services/project/project.service';
import { ModalDialogComponent } from 'src/app/shared/components/modal-dialog/modal-dialog.component';

@Component({
  selector: 'app-programacion-obra-flujo-inversion',
  templateUrl: './programacion-obra-flujo-inversion.component.html',
  styleUrls: ['./programacion-obra-flujo-inversion.component.scss']
})
export class ProgramacionObraFlujoInversionComponent implements OnInit {

  @Input() esFlujoInversion: boolean;
  @Input() contratoConstruccionId: number;
  @Input() contrato: Contrato;
  @Input() proyectoId: number;
  @Input() observacionDevolucionProgramacionObra: number;
  @Input() observacionDevolucionFlujoInversion: number;
  @Input() archivoCargueIdProgramacionObra: number;
  @Input() archivoCargueIdFlujoInversion: number;
  
  @Output() terminoCarga = new EventEmitter();
  @Output() realizoObservacion = new EventEmitter();
  tieneRegistrosObra = true;
  tieneRegistrosInversion = true;

  constructor( 
                private dialog: MatDialog ,
                private faseUnoConstruccionService: FaseUnoConstruccionService,
             ) 
  { }

  ngOnInit(): void {
  }

  openDialog (modalTitle: string, modalText: string) {
    this.dialog.open(ModalDialogComponent, {
      width: '40em',
      data : { modalTitle, modalText }
    });
  };

  cargarProgramacion() {

    if ( this.archivoCargueIdProgramacionObra === undefined && this.esFlujoInversion ){
      this.openDialog( '', 'Se requiere cargar primero la programación de obra' )
      return false;

    }


    const dialogCargarProgramacion = this.dialog.open( DialogCargarProgramacionComponent, {
      width: '75em',
      data: { esFlujoInversion: this.esFlujoInversion, contratoConstruccionId: this.contratoConstruccionId,
              contratoId: this.contrato.contratoId, proyectoId: this.proyectoId, 
              archivoCargueIdProgramacionObra: this.archivoCargueIdProgramacionObra }
    });

    dialogCargarProgramacion.afterClosed().subscribe( response => {
      console.log( 'termino carga masiva?', response );
      this.terminoCarga.emit( response.terminoCarga );
    } );
  }

  esObservacion( realizoObservacion: boolean ) {
    if ( realizoObservacion === true ) {
      this.realizoObservacion.emit( realizoObservacion );
    }
  }

  descargarDRPBoton(){    
    console.log( this.contrato )    
    this.faseUnoConstruccionService.GenerateDRP( this.contrato.contratoId )
      .subscribe((listas:any) => {
        console.log(listas);
        const documento = `DRP ${ this.contrato.numeroContrato }.pdf`;
          const text = documento,
            blob = new Blob([listas], { type: 'application/pdf' }),
            anchor = document.createElement('a');
          anchor.download = documento;
          anchor.href = window.URL.createObjectURL(blob);
          anchor.dataset.downloadurl = ['application/pdf', anchor.download, anchor.href].join(':');
          anchor.click();
    });
  
  }

}
