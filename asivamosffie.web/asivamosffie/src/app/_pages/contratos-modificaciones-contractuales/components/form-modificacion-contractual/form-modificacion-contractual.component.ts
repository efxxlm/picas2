import { identifierName } from '@angular/compiler';
import { Component, OnInit } from '@angular/core';
import { FormGroup, FormBuilder, Validators } from '@angular/forms';
import { ActivatedRoute, Router, UrlSegment } from '@angular/router';
import { CommonService, Dominio } from 'src/app/core/_services/common/common.service';
import { DisponibilidadPresupuestalService } from 'src/app/core/_services/disponibilidadPresupuestal/disponibilidad-presupuestal.service';
import { ProcesosContractualesService } from 'src/app/core/_services/procesosContractuales/procesos-contractuales.service';
import { TipoNovedadCodigo } from 'src/app/_interfaces/estados-novedad.interface';
import { NovedadContractual } from 'src/app/_interfaces/novedadContractual';


@Component({
  selector: 'app-form-modificacion-contractual',
  templateUrl: './form-modificacion-contractual.component.html',
  styleUrls: ['./form-modificacion-contractual.component.scss']
})
export class FormModificacionContractualComponent implements OnInit {

  formModificacionContractual: FormGroup;
  dataNovedad: NovedadContractual;
  tipoModificacion : string;
  modalidadContratoArray: Dominio[] = [];
  modalidadContrato : string;
  esVerDetalle: boolean;
  tipoNovedad = TipoNovedadCodigo;
  adicionBoolean : boolean = false;
  novedadContractualRegistroPresupuestalId: number = 0;

  constructor ( private fb: FormBuilder,
                private activatedRoute: ActivatedRoute,
                private procesosContractualesSvc: ProcesosContractualesService,
                private commonSvc: CommonService,
                private disponibilidadServices: DisponibilidadPresupuestalService
    ) {
    this.crearFormulario();
    this.getNovedadById( this.activatedRoute.snapshot.params.id );
    this.activatedRoute.snapshot.url.forEach( ( urlSegment: UrlSegment ) => {
      if ( urlSegment.path === 'modificacionContractual' ) {
          this.esVerDetalle = false;
          return;
      }
      if ( urlSegment.path === 'detalleModificacionContractual' ) {
          this.esVerDetalle = true;
          return;
      }
    });
  }

  ngOnInit(): void {
  };

  crearFormulario () {
    this.formModificacionContractual = this.fb.group({
      novedadContractualId          : this.activatedRoute.snapshot.params.id,
      numeroOtroSi                  : [ '' ],
      fechaEnvioFirmaContratista    : [ null ],
      fechaFirmaContratista         : [ null ],
      fechaEnvioFirmaFiduciaria     : [ null ],
      fechaFirmaFiduciaria          : [ null ],
      observacionesTramite          : [ null ],
      urlDocumentoSuscrita          : [ null ],
    });
  };

  getNovedadById ( id: number ) {

    this.procesosContractualesSvc.getNovedadById( id )
      .subscribe( novedadContractual => {
        this.dataNovedad = novedadContractual;
        if(this.dataNovedad != null){
          this.formModificacionContractual.patchValue(this.dataNovedad);
        }
        this.dataNovedad.novedadContractualDescripcion.forEach(element => {
            if(this.tipoModificacion == null){
              this.tipoModificacion = element.nombreTipoNovedad;
            }else{
              this.tipoModificacion = this.tipoModificacion + "," + element.nombreTipoNovedad;
            }
            if(element.tipoNovedadCodigo === this.tipoNovedad.adicion)
              this.adicionBoolean = true;
        });
        this.novedadContractualRegistroPresupuestalId = this.dataNovedad?.novedadContractualRegistroPresupuestal[0]?.novedadContractualRegistroPresupuestalId;
        this.commonSvc.modalidadesContrato()
        .subscribe( modalidadContrato => {
          if ( this.dataNovedad.contrato != null) {
            this.modalidadContratoArray = modalidadContrato;
            this.modalidadContrato = this.dataNovedad.contrato.modalidadCodigo !== undefined ? modalidadContrato.filter( modalidad => modalidad.codigo === this.dataNovedad.contrato.modalidadCodigo )[0].nombre : null;
          };
        });
      });

  };

  getDdp(disponibilidadPresupuestalId: number, numeroDdp: string ) {
    this.disponibilidadServices.GenerateDDP(disponibilidadPresupuestalId, true, this.novedadContractualRegistroPresupuestalId,false).subscribe((listas:any) => {
      console.log(listas);
      let documento = '';
        if ( numeroDdp !== undefined ) {
          documento = `${ numeroDdp }.pdf`;
        } else {
          documento = `DDP.pdf`;
        };
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
