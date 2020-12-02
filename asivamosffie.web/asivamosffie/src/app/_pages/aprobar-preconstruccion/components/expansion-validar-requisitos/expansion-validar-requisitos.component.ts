import { Component, OnInit } from '@angular/core';
import { FormBuilder, Validators } from '@angular/forms';
import { MatDialog } from '@angular/material/dialog';
import { ActivatedRoute } from '@angular/router';
import { CommonService, Dominio } from 'src/app/core/_services/common/common.service';
import { FaseUnoPreconstruccionService } from 'src/app/core/_services/faseUnoPreconstruccion/fase-uno-preconstruccion.service';
import { FaseUnoVerificarPreconstruccionService } from 'src/app/core/_services/faseUnoVerificarPreconstruccion/fase-uno-verificar-preconstruccion.service';
import { ModalDialogComponent } from 'src/app/shared/components/modal-dialog/modal-dialog.component';
import { Contrato, ContratoPerfil } from 'src/app/_interfaces/faseUnoPreconstruccion.interface';
import { ObservacionPerfil } from 'src/app/_interfaces/faseUnoVerificarPreconstruccion.interface';
import { FaseUnoAprobarPreconstruccionService } from '../../../../core/_services/faseUnoAprobarPreconstruccion/fase-uno-aprobar-preconstruccion.service';

@Component({
  selector: 'app-expansion-validar-requisitos',
  templateUrl: './expansion-validar-requisitos.component.html',
  styleUrls: ['./expansion-validar-requisitos.component.scss']
})
export class ExpansionValidarRequisitosComponent implements OnInit {

  contrato: Contrato;
  addressForm = this.fb.group({
    tieneObservacion: [ null, Validators.required ],
    observacion: [ null, Validators.required ]
  });
  perfilesCv: Dominio[] = [];
  totalGuardados = 0;
  fechaPoliza: string;
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

  constructor(
    private fb: FormBuilder,
    private activatedRoute: ActivatedRoute,
    private commonSvc: CommonService,
    private dialog: MatDialog,
    private faseUnoAprobarPreconstruccionSvc: FaseUnoAprobarPreconstruccionService,
    private faseUnoPreconstruccionSvc: FaseUnoPreconstruccionService )
  {
    this.getContratacionByContratoId( this.activatedRoute.snapshot.params.id );
  }

  ngOnInit(): void {
  }

  getContratacionByContratoId( pContratoId: string ) {
    this.commonSvc.listaPerfil()
      .subscribe(
        perfiles => {
          this.perfilesCv = perfiles;

          this.faseUnoPreconstruccionSvc.getContratacionByContratoId( pContratoId )
          .subscribe( contrato => {
            this.contrato = contrato;
            const observacionTipo2 = [];
            const observacionTipo3 = [];
            for ( const contratacionProyecto of contrato.contratacion.contratacionProyecto ) {

              let sinDiligenciar = 0;
              let enProceso = 0;
              let completo = 0;

              for ( const perfil of contratacionProyecto.proyecto.contratoPerfil ) {
                // tslint:disable-next-line: no-string-literal
                perfil[ 'tieneObservaciones' ] = null;
                // tslint:disable-next-line: no-string-literal
                perfil[ 'verificarObservacion' ] = '';

                const tipoPerfil = this.perfilesCv.filter( value => value.codigo === perfil.perfilCodigo );
                // tslint:disable-next-line: no-string-literal
                perfil[ 'nombre' ] = tipoPerfil[0].nombre;
                // tslint:disable-next-line: no-string-literal
                if ( perfil[ 'tieneObservacionSupervisor' ] === undefined ) {
                  // tslint:disable-next-line: no-string-literal
                  perfil[ 'estadoSemaforo' ] = 'sin-diligenciar';
                  sinDiligenciar++;
                }
                // tslint:disable-next-line: no-string-literal
                if ( perfil[ 'tieneObservacionSupervisor' ] === false ) {
                  // tslint:disable-next-line: no-string-literal
                  perfil[ 'estadoSemaforo' ] = 'completo';
                  // tslint:disable-next-line: no-string-literal
                  perfil[ 'tieneObservaciones' ] = false;
                  completo++;
                }

                for ( const observacion of perfil.contratoPerfilObservacion ) {
                  if ( observacion.tipoObservacionCodigo === '3' ) {
                    observacionTipo3.push( observacion );
                  }
                  if ( observacion.tipoObservacionCodigo === '2' ) {
                    observacionTipo2.push( observacion );
                  }
                }

                if ( observacionTipo3.length > 0 ) {
                  // tslint:disable-next-line: no-string-literal
                  if (  perfil[ 'tieneObservacionSupervisor' ] === true
                        && observacionTipo3[ observacionTipo3.length - 1 ].observacion === undefined ) {
                    // tslint:disable-next-line: no-string-literal
                    perfil[ 'estadoSemaforo' ] = 'en-proceso';
                    // tslint:disable-next-line: no-string-literal
                    perfil[ 'tieneObservaciones' ] = true;
                    // tslint:disable-next-line: no-string-literal
                    perfil[ 'contratoPerfilObservacionId' ] = observacionTipo3[ observacionTipo3.length - 1 ].contratoPerfilObservacionId;
                    enProceso++;
                  }
                  // tslint:disable-next-line: no-string-literal
                  if (  perfil[ 'tieneObservacionSupervisor' ] === true
                        && observacionTipo3[ observacionTipo3.length - 1 ].observacion !== undefined ) {
                    // tslint:disable-next-line: no-string-literal
                    perfil[ 'estadoSemaforo' ] = 'completo';
                    // tslint:disable-next-line: no-string-literal
                    perfil[ 'tieneObservaciones' ] = true;
                    // tslint:disable-next-line: no-string-literal
                    perfil[ 'contratoPerfilObservacionId' ] = observacionTipo3[ observacionTipo3.length - 1 ].contratoPerfilObservacionId;
                    // tslint:disable-next-line: no-string-literal
                    perfil[ 'verificarObservacion' ] = observacionTipo3[ observacionTipo3.length - 1 ].observacion;
                    completo++;
                  }
                }
                if ( observacionTipo2.length > 0 ) {
                  // tslint:disable-next-line: no-string-literal
                  perfil[ 'observacionApoyo' ] = observacionTipo2[ observacionTipo2.length - 1 ];
                }
              }
              if ( sinDiligenciar === contratacionProyecto.proyecto.contratoPerfil.length ) {
                // tslint:disable-next-line: no-string-literal
                contratacionProyecto[ 'estadoSemaforo' ] = 'sin-diligenciar';
              }
              if ( completo === contratacionProyecto.proyecto.contratoPerfil.length ) {
                // tslint:disable-next-line: no-string-literal
                contratacionProyecto[ 'estadoSemaforo' ] = 'completo';
              }
              if (  enProceso > 0
                    || ( completo > 0 && completo < contratacionProyecto.proyecto.contratoPerfil.length )
                    || ( sinDiligenciar > 0 && sinDiligenciar < contratacionProyecto.proyecto.contratoPerfil.length ) ) {
                // tslint:disable-next-line: no-string-literal
                contratacionProyecto[ 'estadoSemaforo' ] = 'en-proceso';
              }
            }
            console.log( this.contrato );
          } );
        }
      );
  }

  innerObservacion( observacion: string ) {
    if ( observacion !== undefined ) {
      const observacionHtml = observacion.replace( '"', '' );
      return observacionHtml;
    }
  }

  maxLength(e: any, n: number) {
    if (e.editor.getLength() > n) {
      e.editor.deleteText(n, e.editor.getLength());
    }
  }

  textoLimpio(texto: string) {
    if ( texto ){
      const textolimpio = texto.replace(/<[^>]*>/g, '');
      return textolimpio.length > 1000 ? 1000 : textolimpio.length;
    }
  }

  textoLimpioObservacion(texto: string) {
    if ( texto ){
      const textolimpio = texto.replace(/<[^>]*>/g, '');
      return textolimpio;
    }
  }

  openDialog(modalTitle: string, modalText: string) {
    const dialogRef = this.dialog.open(ModalDialogComponent, {
      width: '28em',
      data: { modalTitle, modalText }
    });
  }

  onSubmit( perfil: ContratoPerfil ) {
    const observacionPerfil: ObservacionPerfil = {
      contratoPerfilId: perfil.contratoPerfilId,
      // tslint:disable-next-line: no-string-literal
      observacion: perfil[ 'verificarObservacion' ] === null || perfil[ 'verificarObservacion' ].length === 0 ? null : perfil[ 'verificarObservacion' ],
      // tslint:disable-next-line: no-string-literal
      tieneObservacionSupervisor: perfil[ 'tieneObservaciones' ]
    };
    // tslint:disable-next-line: no-string-literal
    if ( perfil[ 'contratoPerfilObservacionId' ] !== null ) {
      // tslint:disable-next-line: no-string-literal
      observacionPerfil[ 'contratoPerfilObservacionId' ] = perfil[ 'contratoPerfilObservacionId' ];
    }
    console.log( observacionPerfil );
    // tslint:disable-next-line: no-string-literal
    if ( perfil[ 'tieneObservaciones' ] === false && this.totalGuardados === 0 ) {
      this.openDialog( '', '<b>Le recomendamos verificar su respuesta; tenga en cuenta que el apoyo a la supervisión si tuvo observaciones.</b>' );
      this.totalGuardados++;
      return;
    }
    if ( this.totalGuardados === 1 || observacionPerfil.tieneObservacionSupervisor === true ) {
      this.faseUnoAprobarPreconstruccionSvc.aprobarCrearContratoPerfilObservacion( observacionPerfil )
        .subscribe(
          response => {
            this.openDialog( '', response.message );
            this.contrato = null;
            this.getContratacionByContratoId( this.activatedRoute.snapshot.params.id );
          },
          err => this.openDialog( '', err.message )
        );
    }
  }

}
