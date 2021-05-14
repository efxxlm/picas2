import { Component, Inject, OnInit, Pipe } from '@angular/core';
import { MatDialog, MAT_DIALOG_DATA } from '@angular/material/dialog';
import { DevolverPorValidacionComponent } from '../devolver-por-validacion/devolver-por-validacion.component';
import { Router, ActivatedRoute } from '@angular/router';
import { DisponibilidadPresupuestalService } from 'src/app/core/_services/disponibilidadPresupuestal/disponibilidad-presupuestal.service';
import { DomSanitizer } from '@angular/platform-browser';
import { RechasadaPorValidacionComponent } from '../rechasada-por-validacion/rechasada-por-validacion.component';
import { ModalDialogComponent } from 'src/app/shared/components/modal-dialog/modal-dialog.component';
import { TipoDDP } from 'src/app/core/_services/budgetAvailability/budget-availability.service';
import { FormGestionarFuentesAdministrativasComponent } from '../form-gestionar-fuentes-administrativas/form-gestionar-fuentes-administrativas.component';

@Component({
  selector: 'app-validacion-presupuestal',
  templateUrl: './validacion-presupuestal.component.html',
  styleUrls: ['./validacion-presupuestal.component.scss']
})
export class ValidacionPresupuestalComponent implements OnInit {

  valorGestionado = 0;

  constructor(public dialog: MatDialog, private disponibilidadServices: DisponibilidadPresupuestalService,
    private route: ActivatedRoute,
    private router: Router, private sanitized: DomSanitizer,
  ) { }
  detailavailabilityBudget: any = null;
  esModificacion = false;
  pTipoDDP = TipoDDP;

  id;
  esNovedad;
  novedadId;

  ngOnInit(): void {
    console.log(this.pTipoDDP.DDP_tradicional);
    this.id = this.route.snapshot.paramMap.get('id');
    this.esNovedad = this.route.snapshot.paramMap.get('esNovedad');
    this.novedadId = this.route.snapshot.paramMap.get('novedadId');

    if (this.id) {
      //if ( esNovedad === 'true' ){
        this.disponibilidadServices.GetDetailAvailabilityBudgetProyectNew(this.id, this.esNovedad, this.novedadId)
        .subscribe(listas => {
          if (listas.length > 0) {
            this.detailavailabilityBudget = listas[0];
            console.log( this.detailavailabilityBudget )
          if(this.esNovedad){
            if ( this.detailavailabilityBudget.novedadContractual !== undefined ) {
              if ( this.detailavailabilityBudget.novedadContractual.novedadContractualDescripcion !== undefined ) {
                if ( this.detailavailabilityBudget.novedadContractual.novedadContractualDescripcion.length > 0 ) {
                  this.detailavailabilityBudget.novedadContractual.novedadContractualDescripcion.forEach( descripcion => {
                    if ( descripcion.presupuestoAdicionalSolicitado !== undefined ) {
                      this.valorGestionado += descripcion.presupuestoAdicionalSolicitado
                    }
                  } )
                }
              }
            }
          }else{
            if ( this.detailavailabilityBudget.aportantes !== undefined ) {
              if ( this.detailavailabilityBudget.aportantes.length > 0 ) {
                this.detailavailabilityBudget.aportantes.forEach( aportante => {
                  if ( aportante.fuentesFinanciacion !== undefined ) {
                    if ( aportante.fuentesFinanciacion.length > 0 ) {
                      aportante.fuentesFinanciacion.forEach( fuente => {
                        if ( fuente.valor_solicitado_de_la_fuente !== undefined ) {
                          this.valorGestionado += fuente.valor_solicitado_de_la_fuente
                        }
                      } )
                    }
                  }
                } )
              }
            }
          }
          }
        });  
      // }else{
      //   this.disponibilidadServices.GetDetailAvailabilityBudgetProyect(id)
      //   .subscribe(listas => {
      //     console.log(listas);
      //     if (listas.length > 0) {
      //       this.detailavailabilityBudget = listas[0];
      //     }
      //   });
      // }
      
    }
  }
  download() {
    console.log(this.detailavailabilityBudget);
    
    this.disponibilidadServices.GenerateDDP(this.detailavailabilityBudget.id, this.esNovedad, this.novedadId).subscribe((listas: any) => {
      console.log(listas);
      const documento = `${this.detailavailabilityBudget.numeroDDP}.pdf`;
      const text = documento,
        blob = new Blob([listas], { type: 'application/pdf' }),
        anchor = document.createElement('a');
      anchor.download = documento;
      anchor.href = window.URL.createObjectURL(blob);
      anchor.dataset.downloadurl = ['application/pdf', anchor.download, anchor.href].join(':');
      anchor.click();
    });
  }

  openDialogDevolver(tipo, numero, tipoSolicitud) {
    this.dialog.open(DevolverPorValidacionComponent, {
      width: '70em', data: { solicitudID: this.route.snapshot.paramMap.get('id'), tipo: tipo, numeroSolicitud: numero, tipoSolicitud: tipoSolicitud }
    });
  }

  sePuedeValidarFuente() {
    if (this.detailavailabilityBudget) {
      console.log(this.valorGestionado , " - ", this.detailavailabilityBudget.valorSolicitud);
      if (this.detailavailabilityBudget.tipoSolicitudCodigo == this.pTipoDDP.DDP_administrativo ||
          this.detailavailabilityBudget.tipoSolicitudCodigo == this.pTipoDDP.DDP_especial) {
          if (this.valorGestionado == this.detailavailabilityBudget.valorSolicitud) {
            return true;
          }
          else {
            return false;
          }
        }
        else {//para tradicional
          if (this.detailavailabilityBudget.valorSolicitud == this.valorGestionado) {
            return true;
          }
          else {
            return false;
          }
  
        }
    }
    else {
      return false;
    }
  }

  validar() {
    if (!this.sePuedeValidarFuente()) {
      this.openDialog('', '<b>El valor de la solicitud supera el valor de las fuentes de financiación.</b>');
      return false;
    }
    this.disponibilidadServices.SetValidarValidacionDDP(this.id,this.esNovedad,this.novedadId )
    .subscribe(listas => {
      if (listas.code == "200") {
        this.openDialog('', listas.message, true);
      }
      else {
        this.openDialog('', listas.message, false);
      }

    });
  }
  openDialog(modalTitle: string, modalText: string, retorno: boolean = false) {
    let dialogRef = this.dialog.open(ModalDialogComponent, {
      width: '28em',
      data: { modalTitle, modalText }
    });
    dialogRef.afterClosed().subscribe(result => {
      if (retorno)
        this.router.navigate(['/validarDisponibilidadPresupuesto']);

    });
  }
  gestionarFuentes(id: any) {
    console.log(id);
    // this.openDialog('', `El saldo actual de la fuente <b>Recursos propios</b> es menor
    // al valor solicitado de la fuente, verifique por favor.`);
    let dialogRef = this.dialog.open(FormGestionarFuentesAdministrativasComponent, {
      width: '70em', data: { elemento: id, codigo: this.detailavailabilityBudget.numeroSolicitud, ver: this.esModificacion }
    });
    dialogRef.afterClosed().subscribe(result => {
      location.reload();
    });
  }

}
