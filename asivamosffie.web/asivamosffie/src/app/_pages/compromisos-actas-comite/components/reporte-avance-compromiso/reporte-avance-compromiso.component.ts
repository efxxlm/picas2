import { Component, OnInit } from '@angular/core';
import { FormControl, FormBuilder, Validators, FormGroup } from '@angular/forms';
import { MatDialog } from '@angular/material/dialog';
import { Router } from '@angular/router';
import { ModalDialogComponent } from 'src/app/shared/components/modal-dialog/modal-dialog.component';

@Component({
  selector: 'app-reporte-avance-compromiso',
  templateUrl: './reporte-avance-compromiso.component.html',
  styleUrls: ['./reporte-avance-compromiso.component.scss']
})
export class ReporteAvanceCompromisoComponent implements OnInit {

  compromiso       : any;
  reporte          :FormGroup;
  estadoBoolean    : boolean = false;
  sinAvanceBoolean : boolean = false;
  enProcesoBoolean : boolean = false;
  finalizadoBoolean: boolean = false;
  estados          : any[] = [
    { value: 'sinAvance', viewValue: 'Sin iniciar' },
    { value: 'enProceso', viewValue: 'En proceso' },
    { value: 'finalizado', viewValue: 'Finalizada' }
  ];
  editorStyle = {
    height: '100px'
  };
  config = {
    toolbar: [
      ['bold', 'italic', 'underline'],
      [{ list: 'ordered' }, { list: 'bullet' }],
      [{ indent: '-1' }, { indent: '+1' }],
      [{ align: [] }],
    ]
  };

  constructor ( private routes: Router,
                public dialog: MatDialog,
                private fb: FormBuilder ) {
    this.getCompromiso();
    this.crearFormulario();
  }

  ngOnInit(): void {
    this.sinAvanceBoolean  = this.compromiso.estadoCompromiso.sinAvance;
    this.enProcesoBoolean  = this.compromiso.estadoCompromiso.enProceso;
    this.finalizadoBoolean = this.compromiso.estadoCompromiso.finalizado;
  }

  maxLength(e: any, n: number) {
    if (e.editor.getLength() > n) {
      e.editor.deleteText(n, e.editor.getLength());
    }
  }

  textoLimpio(texto: string) {
    if ( texto ){
      const textolimpio = texto.replace(/<[^>]*>/g, '');
      return textolimpio.length;
    };
  };

  crearFormulario(){
    this.reporte = this.fb.group({
      reporteEstado: [ null, Validators.required ]
    })
  };

  getCompromiso () {
    if ( this.routes.getCurrentNavigation().extras.replaceUrl ) {
      this.routes.navigateByUrl( '/compromisosActasComite' );
      return;
    };
    this.compromiso = this.routes.getCurrentNavigation().extras.state.compromiso;
  }

  openDialog(modalTitle: string, modalText: string) {
    this.dialog.open(ModalDialogComponent, {
      width: '28em',
      data: { modalTitle, modalText }
    });
  }

  getEstado ( value: string ) {

    this.estadoBoolean = true;
    
    for ( let estado in this.compromiso.estadoCompromiso ) {
      
      if ( estado === value ) {

        this.compromiso.estadoCompromiso[ estado ] = true;

      } else {
        this.compromiso.estadoCompromiso[ estado ] = false;

      };

    };
    
  };

  onSubmit () {
    
    if ( this.reporte.invalid ) {
      this.openDialog('Falta registrar información', '');
      return;
    };

    this.compromiso.fechaRegistro    = '29/06/2020';
    this.compromiso.gestionRealizada = this.reporte.get( 'reporteEstado' ).value;

    this.openDialog('La información ha sido guardada exitosamente', '');

  }

}
