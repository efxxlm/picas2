import { Component, OnInit } from '@angular/core';
import { FormControl, Validators, FormBuilder } from '@angular/forms';
import { MatDialog } from '@angular/material/dialog';
import { ActivatedRoute } from '@angular/router';
import { CommonService, Dominio } from 'src/app/core/_services/common/common.service';
import { FaseUnoVerificarPreconstruccionService } from 'src/app/core/_services/faseUnoVerificarPreconstruccion/fase-uno-verificar-preconstruccion.service';
import { ModalDialogComponent } from 'src/app/shared/components/modal-dialog/modal-dialog.component';
import { Contrato, ContratoPerfil } from 'src/app/_interfaces/faseUnoPreconstruccion.interface';
import { ObservacionPerfil } from 'src/app/_interfaces/faseUnoVerificarPreconstruccion.interface';
import { FaseUnoAprobarPreconstruccionService } from '../../../../core/_services/faseUnoAprobarPreconstruccion/fase-uno-aprobar-preconstruccion.service';

@Component({
  selector: 'app-expansion-inter-validar-requisitos',
  templateUrl: './expansion-inter-validar-requisitos.component.html',
  styleUrls: ['./expansion-inter-validar-requisitos.component.scss']
})
export class ExpansionInterValidarRequisitosComponent implements OnInit {

  estado: FormControl;
  contrato: Contrato;
  cantidadPerfiles: FormControl;
  perfilesCv: Dominio[] = [];
  fechaPoliza: string;
  totalGuardados = 0;
  addressForm = this.fb.group({
    tieneObservacion: [null, Validators.required],
    observacion: [null, Validators.required]
  });
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
    private faseUnoVerificarPreconstruccionSvc: FaseUnoVerificarPreconstruccionService,
    private dialog: MatDialog,
    private faseUnoAprobarPreconstruccionSvc: FaseUnoAprobarPreconstruccionService,
    private commonSvc: CommonService,
    private activatedRoute: ActivatedRoute )
  {
    this.getContratacionByContratoId( this.activatedRoute.snapshot.params.id );
  }

  ngOnInit(): void {
  }

  getContratacionByContratoId( pContratoId: number ) {
    this.commonSvc.listaPerfil()
      .subscribe(
        response => {
          this.perfilesCv = response;

          this.faseUnoVerificarPreconstruccionSvc.getContratacionByContratoId( pContratoId )
          .subscribe( contrato => {
            this.contrato = contrato;
            const observacionTipo3 = [];
            for ( const contratacionProyecto of contrato.contratacion.contratacionProyecto ) {

              let sinDiligenciar = 0;
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

                for ( const observacionApoyo of perfil.contratoPerfilObservacion ) {
                  if ( observacionApoyo.tipoObservacionCodigo === '3' ) {
                    observacionTipo3.push( observacionApoyo );
                  }
                }

                if ( observacionTipo3.length > 0 ) {
                  // tslint:disable-next-line: no-string-literal
                  if ( perfil[ 'tieneObservacionSupervisor' ] === false ) {
                    // tslint:disable-next-line: no-string-literal
                    perfil[ 'contratoPerfilObservacionId' ] = observacionTipo3[ observacionTipo3.length - 1 ].contratoPerfilObservacionId;
                  }
                  // tslint:disable-next-line: no-string-literal
                  if (  perfil[ 'tieneObservacionSupervisor' ] === true
                        && observacionTipo3[ observacionTipo3.length - 1 ].observacion === undefined ) {
                          // tslint:disable-next-line: no-string-literal
                    perfil[ 'estadoSemaforo' ] = 'en-proceso';
                    // tslint:disable-next-line: no-string-literal
                    perfil[ 'tieneObservaciones' ] = true;
                    // tslint:disable-next-line: no-string-literal
                    perfil[ 'contratoPerfilObservacionId' ] = observacionTipo3[ observacionTipo3.length - 1 ].contratoPerfilObservacionId;
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
              }
              if ( sinDiligenciar === contratacionProyecto.proyecto.contratoPerfil.length ) {
                // tslint:disable-next-line: no-string-literal
                contratacionProyecto[ 'estadoSemaforo' ] = 'sin-diligenciar';
              }
              if ( completo === contratacionProyecto.proyecto.contratoPerfil.length ) {
                // tslint:disable-next-line: no-string-literal
                contratacionProyecto[ 'estadoSemaforo' ] = 'completo';
              }
              if (  ( completo > 0 && completo < contratacionProyecto.proyecto.contratoPerfil.length )
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

  // evalua tecla a tecla
  validateNumberKeypress(event: KeyboardEvent) {
    const alphanumeric = /[0-9]/;
    // tslint:disable-next-line: deprecation
    const inputChar = String.fromCharCode(event.charCode);
    return alphanumeric.test(inputChar) ? true : false;
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

  innerObservacion( observacion: string ) {
    if ( observacion !== undefined ) {
      const observacionHtml = observacion.replace( '"', '' );
      return observacionHtml;
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
      observacion: perfil[ 'verificarObservacion' ] === null ? null : perfil[ 'verificarObservacion' ],
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
