import { Component, OnInit } from '@angular/core';
import { FormControl, FormBuilder, Validators, FormGroup } from '@angular/forms';
import { MatDialog } from '@angular/material/dialog';
import { Router, ActivatedRoute } from '@angular/router';
import { ModalDialogComponent } from 'src/app/shared/components/modal-dialog/modal-dialog.component';
import { CompromisosActasComiteService } from '../../../../core/_services/compromisosActasComite/compromisos-actas-comite.service';

@Component({
  selector: 'app-reporte-avance-compromiso',
  templateUrl: './reporte-avance-compromiso.component.html',
  styleUrls: ['./reporte-avance-compromiso.component.scss']
})
export class ReporteAvanceCompromisoComponent implements OnInit {

  reporte          :FormGroup;
  estadoBoolean    : boolean = false;
  estadoComite     : string = '';
  comite: any = {};
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
                private fb: FormBuilder,
                private activatedRoute: ActivatedRoute,
                private compromisoSvc: CompromisosActasComiteService ) {
    this.getCompromiso( this.activatedRoute.snapshot.params.id );
    this.crearFormulario();
  }

  ngOnInit(): void {
  };

  getCompromiso ( id: number ) {
    this.compromisoSvc.getCompromiso( id )
      .subscribe( ( resp: any ) => {
        this.estadoComite = resp[0].estadoCompromisoText;
        this.comite = resp[0];
        console.log( this.comite );
      } );
  };

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

  openDialog(modalTitle: string, modalText: string) {
    this.dialog.open(ModalDialogComponent, {
      width: '28em',
      data: { modalTitle, modalText }
    });
  }

  getEstado ( value: string ) {

    console.log( value );
    
  };

  onSubmit () {
    
    if ( this.reporte.invalid ) {
      this.openDialog('Falta registrar información', '');
      return;
    };

    const seguimiento = {
      DescripcionSeguimiento: this.reporte.get( 'reporteEstado' ).value,
      SesionComiteTecnicoCompromisoId: this.comite.sesionComiteTecnicoCompromisoId
    }

    this.compromisoSvc.postCompromisos( seguimiento, this.comite.comiteTecnicoId )
      .subscribe( resp => {
        console.log( resp );
        this.openDialog('La información ha sido guardada exitosamente', '');
      } );

  }

}
