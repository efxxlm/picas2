import { Component, OnInit } from '@angular/core';
import { FormBuilder, Validators, FormGroup, FormArray } from '@angular/forms';
import { ActivatedRoute } from '@angular/router';
import { Contrato } from 'src/app/_interfaces/faseUnoPreconstruccion.interface';
import { FaseUnoPreconstruccionService } from '../../../../core/_services/faseUnoPreconstruccion/fase-uno-preconstruccion.service';
import { ObservacionPerfil } from '../../../../_interfaces/faseUnoVerificarPreconstruccion.interface';
import { ContratoPerfil } from '../../../../_interfaces/faseUnoPreconstruccion.interface';
import { FaseUnoVerificarPreconstruccionService } from '../../../../core/_services/faseUnoVerificarPreconstruccion/fase-uno-verificar-preconstruccion.service';
import { MatDialog } from '@angular/material/dialog';
import { ModalDialogComponent } from 'src/app/shared/components/modal-dialog/modal-dialog.component';

@Component({
  selector: 'app-expansion-verificar-requisitos',
  templateUrl: './expansion-verificar-requisitos.component.html',
  styleUrls: ['./expansion-verificar-requisitos.component.scss']
})
export class ExpansionVerificarRequisitosComponent implements OnInit {

  contrato: Contrato;
  addressForm = this.fb.group({
    tieneObservacion: [null, Validators.required],
    observacion: [null, Validators.required]
  });
  proyectosForm: any[] = [];

  editorStyle = {
    height: '45px'
  };

  config = {
    toolbar: [
      ['bold', 'italic', 'underline'],
      [{ list: 'ordered' }, { list: 'bullet' }],
      [{ indent: '-1' }, { indent: '+1' }],
      [{ align: [] }],
    ]
  };

  constructor ( private fb: FormBuilder,
                private activatedRoute: ActivatedRoute,
                private dialog: MatDialog,
                private faseUnoVerificarPreconstruccionSvc: FaseUnoVerificarPreconstruccionService,
                private faseUnoPreconstruccionSvc: FaseUnoPreconstruccionService ) 
  {
    this.getContratacionByContratoId( this.activatedRoute.snapshot.params.id );
  }

  ngOnInit(): void {
  }

  getContratacionByContratoId ( pContratoId: string ) {
    this.faseUnoPreconstruccionSvc.getContratacionByContratoId( pContratoId )
      .subscribe( contrato => {
        this.contrato = contrato;
        for ( let contratacionProyecto of contrato.contratacion.contratacionProyecto ) {

          for ( let perfil of contratacionProyecto.proyecto.contratoPerfil ) {
            perfil[ 'tieneObservaciones' ] = null;
            perfil[ 'verificarObservacion' ] = '';
          };

        };
        console.log( this.contrato );
      } );
  };

  maxLength(e: any, n: number) {
    if (e.editor.getLength() > n) {
      e.editor.deleteText(n, e.editor.getLength());
    }
  };

  openDialog(modalTitle: string, modalText: string) {
    let dialogRef =this.dialog.open(ModalDialogComponent, {
      width: '28em',
      data: { modalTitle, modalText }
    });   
  };

  textoLimpio(texto: string) {
    if ( texto ){
      const textolimpio = texto.replace(/<[^>]*>/g, '');
      return textolimpio.length;
    }
  };

  textoLimpioObservacion(texto: string) {
    if ( texto ){
      const textolimpio = texto.replace(/<[^>]*>/g, '');
      return textolimpio;
    }
  }

  onSubmit( perfil: ContratoPerfil ) {
    const observacionPerfil: ObservacionPerfil = {
      contratoPerfilId: perfil.contratoPerfilId,
      observacion: perfil[ 'verificarObservacion' ]
    };
    console.log( observacionPerfil );
    this.faseUnoVerificarPreconstruccionSvc.crearContratoPerfilObservacion( observacionPerfil )
      .subscribe(
        response => this.openDialog( '', response.message ),
        err => this.openDialog( '', err.message )
      );
  };

}
