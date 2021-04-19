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
  estaEditando = false;

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
            for ( const contratacionProyecto of contrato.contratacion.contratacionProyecto ) {
              const observacionTipo3 = [];
              let sinDiligenciar = 0;
              let completo = 0;

              for ( const perfil of contratacionProyecto.proyecto.contratoPerfil ) {
                perfil[ 'tieneObservaciones' ] = null;
                perfil[ 'verificarObservacion' ] = null;

                const tipoPerfil = this.perfilesCv.filter( value => value.codigo === perfil.perfilCodigo );
                perfil[ 'nombre' ] = tipoPerfil[0] !== undefined ? tipoPerfil[0].nombre : '';
                if ( perfil[ 'tieneObservacionSupervisor' ] === undefined ) {
                  perfil[ 'estadoSemaforo' ] = 'sin-diligenciar';
                  sinDiligenciar++;
                }
                if ( perfil[ 'tieneObservacionSupervisor' ] === false ) {
                  perfil[ 'estadoSemaforo' ] = 'completo';
                  perfil[ 'tieneObservaciones' ] = false;
                  completo++;
                }

                for ( const observacionApoyo of perfil.contratoPerfilObservacion ) {
                  if ( observacionApoyo.tipoObservacionCodigo === '3' ) {
                    observacionTipo3.push( observacionApoyo );
                  }
                }

                if ( observacionTipo3.length > 0 ) {
                  if (  perfil[ 'tieneObservacionSupervisor' ] === true
                        && observacionTipo3[ observacionTipo3.length - 1 ].observacion === undefined ) {
                    perfil[ 'estadoSemaforo' ] = 'en-proceso';
                    perfil[ 'tieneObservaciones' ] = true;
                    perfil[ 'contratoPerfilObservacionId' ] = observacionTipo3[ observacionTipo3.length - 1 ].contratoPerfilObservacionId;
                  }
                  if (  perfil[ 'tieneObservacionSupervisor' ] === true
                        && observacionTipo3[ observacionTipo3.length - 1 ].observacion !== undefined ) {
                    perfil[ 'estadoSemaforo' ] = 'completo';
                    perfil[ 'tieneObservaciones' ] = true;
                    perfil[ 'contratoPerfilObservacionId' ] = observacionTipo3[ observacionTipo3.length - 1 ].contratoPerfilObservacionId;
                    perfil[ 'verificarObservacion' ] = observacionTipo3[ observacionTipo3.length - 1 ].observacion;
                    this.estaEditando = true;
                    this.addressForm.markAllAsTouched();
                    completo++;
                  }
                }
              }
              if ( sinDiligenciar === contratacionProyecto.proyecto.contratoPerfil.length ) {
                contratacionProyecto[ 'estadoSemaforo' ] = 'sin-diligenciar';
              }
              if ( completo === contratacionProyecto.proyecto.contratoPerfil.length ) {
                contratacionProyecto[ 'estadoSemaforo' ] = 'completo';
              }
              if (  ( completo > 0 && completo < contratacionProyecto.proyecto.contratoPerfil.length )
                    || ( sinDiligenciar > 0 && sinDiligenciar < contratacionProyecto.proyecto.contratoPerfil.length ) ) {
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
      e.editor.deleteText(n - 1, e.editor.getLength());
    }
  }

  textoLimpio( evento: any, n: number ) {
    if ( evento !== undefined ) {
      return evento.getLength() > n ? n : evento.getLength();
    } else {
      return 0;
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
    this.estaEditando = true;
    this.addressForm.markAllAsTouched();
    const observacionPerfil: ObservacionPerfil = {
      contratoPerfilId: perfil.contratoPerfilId,
      observacion: perfil[ 'verificarObservacion' ] === null || perfil[ 'verificarObservacion' ].length === 0 ? null : perfil[ 'verificarObservacion' ],
      tieneObservacionSupervisor: perfil[ 'tieneObservaciones' ]
    };
    if ( perfil[ 'contratoPerfilObservacionId' ] !== null ) {
      observacionPerfil[ 'contratoPerfilObservacionId' ] = perfil[ 'contratoPerfilObservacionId' ];
    }
    console.log( observacionPerfil );
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
