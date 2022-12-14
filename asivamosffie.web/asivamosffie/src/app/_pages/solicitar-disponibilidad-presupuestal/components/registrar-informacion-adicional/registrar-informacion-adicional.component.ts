import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Params, Router } from '@angular/router';
import { FormBuilder, Validators, FormsModule } from '@angular/forms';
import { NgModule } from '@angular/core';
import { DisponibilidadPresupuestal } from 'src/app/_interfaces/budgetAvailability';
import { BudgetAvailabilityService } from 'src/app/core/_services/budgetAvailability/budget-availability.service';
import { MatDialog } from '@angular/material/dialog';
import { ModalDialogComponent } from 'src/app/shared/components/modal-dialog/modal-dialog.component';
import { ProjectContractingService } from 'src/app/core/_services/projectContracting/project-contracting.service';
import { ProjectService, Proyecto } from 'src/app/core/_services/project/project.service';
import { Contratacion } from 'src/app/_interfaces/project-contracting';
import { ContractualNoveltyService } from 'src/app/core/_services/ContractualNovelty/contractual-novelty.service';
import { NovedadContractual } from 'src/app/_interfaces/novedadContractual';

@Component({
  selector: 'app-registrar-informacion-adicional',
  templateUrl: './registrar-informacion-adicional.component.html',
  styleUrls: ['./registrar-informacion-adicional.component.scss']
})
export class RegistrarInformacionAdicionalComponent implements OnInit {
  objetoDisponibilidad: DisponibilidadPresupuestal = {};
  listaProyectos: Proyecto[] = [];
  idNovedad: number;
  novedadContractual: NovedadContractual;
  objetoNovedad: any;
  observacionesNovedad: any[];

  addressForm = this.fb.group({
    // plazoMeses: [null, Validators.required],
    // plazoDias: [null, Validators.required],
    objeto: [null, Validators.required]
  });

  editorStyle = {
    height: '45px'
  };

  config = {
    toolbar: [
      ['bold', 'italic', 'underline'],
      [{ list: 'ordered' }, { list: 'bullet' }],
      [{ indent: '-1' }, { indent: '+1' }],
      [{ align: [] }]
    ]
  };
  observaciones: any[];
  estaEditando = false;
  tipoSolicitudCodigo;
  contratacion: Contratacion;
  ddpsolicitud: any;
  ddpvalor: any;
  ddpdetalle: any;

  constructor(
    private fb: FormBuilder,
    private activatedroute: ActivatedRoute,
    private budgetAvailabilityService: BudgetAvailabilityService,
    public dialog: MatDialog,
    private router: Router,
    private projectContractingService: ProjectContractingService,
    private projectService: ProjectService,
    private novedadContractualService: ContractualNoveltyService
  ) {
    this.activatedroute.params.subscribe((params: Params) => {
      console.log(params);
      this.objetoDisponibilidad.contratacionId = params.idContratacion;
      this.objetoDisponibilidad.disponibilidadPresupuestalId = params.idDisponibilidadPresupuestal;
      this.objetoDisponibilidad.tipoSolicitudCodigo = params.idTipoSolicitud;
      this.idNovedad = params.idNovedad;
      console.log(this.objetoDisponibilidad);
      if (this.objetoDisponibilidad.disponibilidadPresupuestalId > 0) {
        this.cargarDisponibilidadPre();
      } else {
        this.cargarDisponibilidadNueva();
      }
      this.getNovedad(this.idNovedad);
    });
  }

  cargarDisponibilidadPre() {
    this.budgetAvailabilityService
      .getDisponibilidadPresupuestalById(this.objetoDisponibilidad.disponibilidadPresupuestalId)
      .subscribe(response => {
        this.objetoDisponibilidad = response;
        this.addressForm.get('objeto').setValue(this.objetoDisponibilidad.objeto);
        // this.addressForm.get('plazoMeses').setValue(this.objetoDisponibilidad.plazoMeses);
        // this.addressForm.get('plazoDias').setValue(this.objetoDisponibilidad.plazoDias);
        this.observaciones = response.disponibilidadPresupuestalObservacion;
        this.projectContractingService
          .getContratacionByContratacionId(this.objetoDisponibilidad.contratacionId)
          .subscribe(contratacion => {
            this.contratacion = contratacion;
            this.tipoSolicitudCodigo = contratacion.tipoSolicitudCodigo;
            contratacion.contratacionProyecto.forEach(cp => {
              cp.proyecto.contratacionProyectoAportante = cp.contratacionProyectoAportante;
              this.listaProyectos.push(cp.proyecto);
            });
          });
      });
  }

  cargarDisponibilidadNueva() {
    this.objetoDisponibilidad.disponibilidadPresupuestalProyecto = [];

    this.budgetAvailabilityService.getReuestCommittee().subscribe(listaSolicitudes => {
      listaSolicitudes.forEach(solicitud => {
        if (solicitud.contratacionId == this.objetoDisponibilidad.contratacionId) {
          this.objetoDisponibilidad.fechaSolicitud = solicitud.fechaSolicitud;
          this.objetoDisponibilidad.numeroSolicitud = solicitud.numeroSolicitud;
          this.objetoDisponibilidad.opcionContratarCodigo = solicitud.opcionContratar;
          this.objetoDisponibilidad.valorSolicitud = solicitud.valorSolicitud;
          this.objetoDisponibilidad.tipoSolicitudCodigo = solicitud.tipoSolicitudCodigo
            ? solicitud.tipoSolicitudCodigo
            : this.objetoDisponibilidad.tipoSolicitudCodigo;
        }
      });
      this.projectContractingService
        .getContratacionByContratacionId(this.objetoDisponibilidad.contratacionId)
        .subscribe(
          contratacion => {
            console.log(contratacion);
            this.contratacion = contratacion;
            this.tipoSolicitudCodigo = contratacion.tipoSolicitudCodigo;
            this.objetoDisponibilidad.fechaComiteTecnicoNotMapped = contratacion.fechaComiteTecnicoNotMapped;
            contratacion.contratacionProyecto.forEach(cp => {
              cp.proyecto.contratacionProyectoAportante = cp.contratacionProyectoAportante;

              this.listaProyectos.push(cp.proyecto);

              let plazoMesesObra = 0;
              let plazoMesesInterventoria = 0;
              let plazoDiasObra = 0;
              let plazoDiasInterventoria = 0;

              // obra
              // if (this.tipoSolicitudCodigo === '1') {

              //   contratacion.contratacionProyecto.forEach(cp => {
              //     if (plazoDiasObra < cp.proyecto.plazoDiasObra)
              //       plazoDiasObra = cp.proyecto.plazoDiasObra;

              //     if (plazoMesesObra < cp.proyecto.plazoMesesObra)
              //       plazoMesesObra = cp.proyecto.plazoMesesObra;
              //   });

              //   this.addressForm.get("plazoMeses").setValue(plazoMesesObra);
              //   this.addressForm.get("plazoDias").setValue(plazoDiasObra);
              // } else {

              //   contratacion.contratacionProyecto.forEach(cp => {
              //     if (plazoDiasInterventoria < cp.proyecto.plazoDiasInterventoria)
              //       plazoDiasInterventoria = cp.proyecto.plazoDiasInterventoria;

              //     if (plazoMesesInterventoria < cp.proyecto.plazoMesesInterventoria)
              //       plazoMesesInterventoria = cp.proyecto.plazoMesesInterventoria;
              //   });

              //   this.addressForm.get("plazoMeses").setValue(plazoMesesInterventoria);
              //   this.addressForm.get("plazoDias").setValue(plazoDiasInterventoria);
              // }

              if (this.objetoDisponibilidad.tipoSolicitudCodigo == '2') {
                //modificacionContractual
                let plazoMesesNovedad = 0;
                let plazoDiasNovedad = 0;

                let plazoMesesObra = 0;
                let plazoMesesInterventoria = 0;
                let plazoDiasObra = 0;
                let plazoDiasInterventoria = 0;

                this.budgetAvailabilityService
                  .getNovedadContractual(this.objetoDisponibilidad.contratacionId)
                  .subscribe(
                    res => {
                      console.log(res);

                      res = res.filter(x => x.novedadContractualId == this.idNovedad);

                      this.ddpsolicitud = res[0].contrato.contratacion.disponibilidadPresupuestal[0].numeroDdp;
                      this.ddpvalor =
                        res[0].contrato.contratacion.disponibilidadPresupuestal[0].valorTotalDisponibilidad;
                      this.ddpdetalle = res[0].novedadContractualDescripcion[0].resumenJustificacion;
                      this.objetoDisponibilidad.novedadContractualId = res[0].novedadContractualId;
                      this.objetoDisponibilidad.esNovedadContractual = true;
                      this.objetoDisponibilidad.numeroSolicitud = res[0].numeroSolicitud;
                      this.objetoDisponibilidad.tipoSolicitudCodigo = '1'; //tradicional
                      this.objetoDisponibilidad.valorSolicitud =
                        res[0].novedadContractualDescripcion[0].presupuestoAdicionalSolicitado;

                      res[0].novedadContractualDescripcion.forEach(novedad => {
                        // Prorroga a la suspension - Prorroga - suspension
                        if (
                          novedad.tipoNovedadCodigo === '2' ||
                          novedad.tipoNovedadCodigo === '4' ||
                          novedad.tipoNovedadCodigo === '1'
                        ) {
                          if (novedad.plazoAdicionalMeses !== undefined) plazoMesesNovedad = novedad.plazoAdicionalDias;
                          if (novedad.plazoAdicionalDias !== undefined) plazoDiasNovedad = novedad.plazoAdicionalDias;
                        }
                      });

                      // obra
                      if (this.tipoSolicitudCodigo === '1') {
                        contratacion.contratacionProyecto.forEach(cp => {
                          if (plazoDiasObra < cp.proyecto.plazoDiasObra) plazoDiasObra = cp.proyecto.plazoDiasObra;

                          if (plazoMesesObra < cp.proyecto.plazoMesesObra) plazoMesesObra = cp.proyecto.plazoMesesObra;
                        });

                        // this.addressForm.get("plazoMeses").setValue(plazoMesesNovedad !== 0 ? plazoMesesNovedad : plazoMesesObra);
                        // this.addressForm.get("plazoDias").setValue(plazoDiasNovedad !== 0 ? plazoDiasNovedad : plazoDiasObra);
                      } else {
                        contratacion.contratacionProyecto.forEach(cp => {
                          if (plazoDiasInterventoria < cp.proyecto.plazoDiasInterventoria)
                            plazoDiasInterventoria = cp.proyecto.plazoDiasInterventoria;

                          if (plazoMesesInterventoria < cp.proyecto.plazoMesesInterventoria)
                            plazoMesesInterventoria = cp.proyecto.plazoMesesInterventoria;
                        });

                        // this.addressForm.get("plazoMeses").setValue(plazoMesesNovedad !== 0 ? plazoMesesNovedad : plazoMesesInterventoria);
                        // this.addressForm.get("plazoDias").setValue(plazoDiasNovedad !== 0 ? plazoDiasNovedad : plazoDiasInterventoria);
                      }

                      this.addressForm
                        .get('objeto')
                        .setValue(res[0].novedadContractualDescripcion[0].resumenJustificacion);
                    },
                    err => {
                      console.log(err);
                    }
                  );
              }
            });
            console.log(this.listaProyectos);
          },
          err => {
            console.log(err);
          }
        ),
        err => {
          console.log(err);
        };
    });
  }

  ngOnInit(): void {}

  maxLength(e: any, n: number) {
    if (e.editor.getLength() > n) {
      e.editor.deleteText(n, e.editor.getLength());
    }
  }

  // textoLimpio(texto: string) {
  //   let saltosDeLinea = 0;
  //   saltosDeLinea += this.contarSaltosDeLinea(texto, '<p');
  //   saltosDeLinea += this.contarSaltosDeLinea(texto, '<li');

  //   if (texto) {
  //     const textolimpio = texto.replace(/<(?:.|\n)*?>/gm, '');
  //     return textolimpio.length + saltosDeLinea;
  //   }
  // }

  textoLimpio(evento: any, n: number) {
    if (evento !== undefined) {
      return evento.getLength() > n ? n : evento.getLength();
    } else {
      return 0;
    }
  }

  private contarSaltosDeLinea(cadena: string, subcadena: string) {
    let contadorConcurrencias = 0;
    let posicion = 0;
    while ((posicion = cadena.indexOf(subcadena, posicion)) !== -1) {
      ++contadorConcurrencias;
      posicion += subcadena.length;
    }
    return contadorConcurrencias;
  }

  openDialog(modalTitle: string, modalText: string) {
    const dialogRef = this.dialog.open(ModalDialogComponent, {
      width: '28em',
      data: { modalTitle, modalText }
    });
  }

  onSubmit() {
    this.estaEditando = true;
    this.addressForm.markAllAsTouched();
    if (this.addressForm.valid) {
      // let plazoObra: number = 0;
      // let plazoInterventoria: number = 0;
      // let plazo: number = 0;

      // plazo = this.addressForm.get('plazoMeses').value * 30;
      // plazo = plazo + this.addressForm.get('plazoDias').value;

      // console.log(this.tipoSolicitudCodigo, plazo)
      // // obra
      // if (this.tipoSolicitudCodigo == '1') {
      //   this.contratacion.contratacionProyecto.forEach(cp => {
      //     console.log(cp.proyecto.plazoMesesObra, cp.proyecto.plazoDiasObra);
      //     let plazoTemp = 0;
      //     plazoTemp = cp.proyecto.plazoMesesObra * 30;
      //     plazoTemp = Number(plazoTemp) + Number(cp.proyecto.plazoDiasObra);

      //     if (plazoTemp > plazoObra)
      //       plazoObra = plazoTemp;

      //   });

      //   if (plazo < plazoObra) {
      //     console.log(plazoObra, plazo);
      //     this.openDialog('', '<b> El plazo no puede ser menor al del proyecto. </b>')
      //     return false;
      //   }

      // }
      // if (this.tipoSolicitudCodigo == '2') {
      //   this.contratacion.contratacionProyecto.forEach(cp => {
      //     console.log(cp.proyecto.plazoMesesInterventoria, cp.proyecto.plazoDiasInterventoria);
      //     let plazoTemp = 0;
      //     plazoTemp = cp.proyecto.plazoMesesInterventoria * 30;
      //     plazoTemp = Number(plazoTemp) + Number(cp.proyecto.plazoDiasInterventoria);

      //     if (plazoTemp > plazoInterventoria)
      //       plazoInterventoria = plazoTemp;

      //   });

      //   if (plazo < plazoInterventoria) {
      //     console.log(plazoInterventoria, plazo);
      //     this.openDialog('', '<b> El plazo no puede ser menor al del proyecto. </b>')
      //     return false;
      //   }

      // }

      this.objetoDisponibilidad.objeto = this.addressForm.get('objeto').value;
      // this.objetoDisponibilidad.plazoMeses = this.addressForm.get('plazoMeses').value;
      // this.objetoDisponibilidad.plazoDias = this.addressForm.get('plazoDias').value;

      console.log(this.objetoDisponibilidad);

      if (this.objetoDisponibilidad.esNovedadContractual === true) {
        const registroPresupuesta = {
          //novedadContractualRegistroPresupuestalId = this.objetoDisponibilidad.NovedadContractualRegistroPresupuestalId,
          novedadContractualId: this.objetoDisponibilidad.novedadContractualId,
          disponibilidadPresupuestalId: this.objetoDisponibilidad.disponibilidadPresupuestalId,
          numeroSolicitud: this.objetoDisponibilidad.numeroSolicitud,
          valorSolicitud: this.objetoDisponibilidad.valorSolicitud,
          estadoSolicitudCodigo: this.objetoDisponibilidad.estadoSolicitudCodigo,
          objeto: this.objetoDisponibilidad.objeto,
          eliminado: this.objetoDisponibilidad.eliminado,
          fechaDdp: this.objetoDisponibilidad.fechaDdp,
          numeroDrp: this.objetoDisponibilidad.numeroDrp,
          plazoMeses: this.contratacion.plazoContratacion.plazoMeses,
          plazoDias: this.contratacion.plazoContratacion.plazoDias
        };

        this.budgetAvailabilityService
          .createOrEditInfoAdditionalNoveltly(registroPresupuesta, this.objetoDisponibilidad.contratacionId)
          .subscribe(respuesta => {
            this.openDialog('', `<b>${respuesta.message}</b>`);
            if (respuesta.code == '200')
              this.router.navigate(['/solicitarDisponibilidadPresupuestal/crearSolicitudTradicional']);
          });
      } else {
        this.budgetAvailabilityService.createOrEditInfoAdditional(this.objetoDisponibilidad).subscribe(respuesta => {
          this.openDialog('', `<b>${respuesta.message}</b>`);
          if (respuesta.code == '200')
            this.router.navigate(['/solicitarDisponibilidadPresupuestal/crearSolicitudTradicional']);
        });
      }
    } else {
      this.openDialog('', '<b>Por favor ingrese todos los campos obligatorios.</b>');
    }

    console.log(this.objetoDisponibilidad);
  }

  getNovedad(novedadContractualId) {
    if (novedadContractualId > 0) {
      this.novedadContractualService.getNovedadContractualById(novedadContractualId).subscribe(novedad => {
        this.novedadContractual = novedad;
        if (this.novedadContractual?.novedadContractualDescripcion.length > 0) {
          this.novedadContractual?.novedadContractualDescripcion.forEach(novedad => {
            if (novedad.tipoNovedadCodigo == '3') {
              this.objetoNovedad = novedad.resumenJustificacion;
            }
          });
        }
        if (this.novedadContractual?.novedadContractualObservaciones.length > 0) {
          this.observacionesNovedad = this.novedadContractual?.novedadContractualObservaciones;
        }
      });
    }
  }
}
