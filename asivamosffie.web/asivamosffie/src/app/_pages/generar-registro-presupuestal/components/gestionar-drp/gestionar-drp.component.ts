import { Component, OnInit } from '@angular/core';
import { MatTableDataSource } from '@angular/material/table';
import { MatDialog, MatDialogRef, MatDialogConfig, MAT_DIALOG_DATA } from '@angular/material/dialog';
import { CancelarDrpComponent } from '../cancelar-drp/cancelar-drp.component';
import { DisponibilidadPresupuestalService } from 'src/app/core/_services/disponibilidadPresupuestal/disponibilidad-presupuestal.service';
import { ActivatedRoute, Router, UrlSegment } from '@angular/router';
import { DomSanitizer } from '@angular/platform-browser';
import { ModalDialogComponent } from 'src/app/shared/components/modal-dialog/modal-dialog.component';
import { CurrencyPipe } from '@angular/common';
import { THIS_EXPR } from '@angular/compiler/src/output/output_ast';

export interface tablaEjemplo {
  componente: string;
  uso: any[];
  valorUso: any[];
  valorTotal: string;
}

const ELEMENT_DATA: tablaEjemplo[] = [
  {
    componente: "Obra", uso: [
      { nombre: "Diseño" }, { nombre: "Diagnostico" }, { nombre: "Obra Principal" }
    ], valorUso: [
      { valor: "$ 8.000.000" }, { valor: "$ 12.000.000" }, { valor: "$ 60.000.000" }
    ], valorTotal: '$80.000.000'
  },
];
@Component({
  selector: 'app-gestionar-drp',
  templateUrl: './gestionar-drp.component.html',
  styleUrls: ['./gestionar-drp.component.scss']
})
export class GestionarDrpComponent implements OnInit {
  listacomponentes: any[] = [];
  public numContrato = "";
  public fechaContrato = "";
  public solicitudContrato = "";
  public estadoSolicitud = "";
  displayedColumns: string[] = ['componente', 'fase','uso', 'valorUso', 'fuenteFinanciacion', 'valorTotal'];
  esModificacion = false;
  dataSource = [];
  detailavailabilityBudget: any;
  listaComponentesUsoAportante:any[] = [];
  esNovedad;
  novedadId;
  esLiberacion: boolean = false;

  constructor(public dialog: MatDialog, private disponibilidadServices: DisponibilidadPresupuestalService,
    private route: ActivatedRoute, private currencyPipe: CurrencyPipe,
    private router: Router, private sanitized: DomSanitizer,) {
      this.route.snapshot.url.forEach( ( urlSegment: UrlSegment ) => {
        if ( urlSegment.path === 'conLiberacionSaldo' ){
          this.esLiberacion = true;
          return;
        }
    } );
     }

  openDialog(modalTitle: string, modalText: string, relocate = false) {
    let dialogref = this.dialog.open(ModalDialogComponent, {
      width: '28em',
      data: { modalTitle, modalText }
    });
    if (relocate) {
      dialogref.afterClosed().subscribe(result => {
        this.router.navigate(["/generarRegistroPresupuestal"], {});
      });
    }
  }
  ngOnInit(): void {
    let listaComponentesUsoAportante=[];
    let dataSource = [];
    const id = this.route.snapshot.paramMap.get('id');
    this.esNovedad = this.route.snapshot.paramMap.get('esNovedad');
    this.novedadId = this.route.snapshot.paramMap.get('novedadId');

    if (id) {
      if(this.esLiberacion != true){
        this.disponibilidadServices.GetDetailAvailabilityBudgetProyectNew(id, this.esNovedad, this.novedadId, true)
        .subscribe(listas => {

        if (listas.length > 0) {
          this.detailavailabilityBudget = listas[0];
          console.log( this.detailavailabilityBudget )
          this.detailavailabilityBudget.proyectos.forEach(element => {
            listaComponentesUsoAportante = [];
            element.aportantes.forEach(aportante => {
              this.listacomponentes = [];
              // filtro por aportante
              element.componenteGrilla.filter( r => r.cofinanciacionAportanteId == aportante.cofinanciacionAportanteId).forEach(element2 => {
                this.listacomponentes.push({
                  componente: element2.componente,
                  fase: element2?.fase,
                  uso: [{ nombre: element2.uso }],
                  valorUso: [{ valor: element2.valorUso.map(y => { let convert = y.toString().replace(/(\d)(?=(\d{3})+(?!\d))/g, "$1,"); return "$" + convert; }) }],
                  valorTotal: element2.valorTotal,
                  fuenteFinanciacion: aportante.fuentesFinanciacion[0].fuente
                });
              });
              //dataSource.push(new MatTableDataSource(this.listacomponentes));
              listaComponentesUsoAportante.push( new MatTableDataSource(this.listacomponentes) )
            });
            element['listaComponentesUsoAportante'] = listaComponentesUsoAportante;
          });

        }
        else {
          this.openDialog('', 'Error al intentar recuperar los datos de la solicitud, por favor intenta nuevamente.');
        }
        });
      }else{
        this.disponibilidadServices.GetDetailAvailabilityBudgetProyectHistorical(id, this.esNovedad, this.novedadId, true)
        .subscribe(listas => {
        if (listas.length > 0) {
          this.detailavailabilityBudget = listas[0];
          this.detailavailabilityBudget.proyectos.forEach(element => {
            listaComponentesUsoAportante = [];
            element.aportantes.forEach(aportante => {
              this.listacomponentes = [];
              // filtro por aportante
              element.componenteGrilla.filter( r => r.cofinanciacionAportanteId == aportante.cofinanciacionAportanteId).forEach(element2 => {
                this.listacomponentes.push({
                  componente: element2.componente,
                  fase: element2?.fase,
                  uso: [{ nombre: element2.uso }],
                  valorUso: [{ valor: element2.valorUso.map(y => { let convert = y.toString().replace(/(\d)(?=(\d{3})+(?!\d))/g, "$1,"); return "$" + convert; }) }],
                  valorTotal: element2.valorTotal,
                  fuenteFinanciacion: aportante.fuentesFinanciacion[0].fuente
                });
              });
              //dataSource.push(new MatTableDataSource(this.listacomponentes));
              listaComponentesUsoAportante.push( new MatTableDataSource(this.listacomponentes) )
            });
            element['listaComponentesUsoAportante'] = listaComponentesUsoAportante;
          });

        }
        else {
          this.openDialog('', 'Error al intentar recuperar los datos de la solicitud, por favor intenta nuevamente.');
        }
      });
      }
    }
    //this.dataSource = new MatTableDataSource(ELEMENT_DATA);
  }


  cancelarDRPBoton() {
    const dialogConfig = new MatDialogConfig();
    dialogConfig.height = 'auto';
    dialogConfig.width = '50%';
    const dialogRef = this.dialog.open(CancelarDrpComponent, dialogConfig);
    dialogRef.componentInstance.id = this.detailavailabilityBudget.id;
    dialogRef.componentInstance.tipo = this.detailavailabilityBudget.tipoSolicitudEspecial;
    dialogRef.componentInstance.nSolicitud = this.detailavailabilityBudget.numeroSolicitud;
    dialogRef.componentInstance.fecha = this.detailavailabilityBudget.fechaSolicitud;
    dialogRef.afterClosed().subscribe(result => {
      this.router.navigate(["/generarRegistroPresupuestal"], {});
    });
  }

  descargarDDPBoton() {
    console.log(this.detailavailabilityBudget);
    this.disponibilidadServices.GenerateDDP(this.detailavailabilityBudget.id, this.esNovedad, this.novedadId,false,this.esLiberacion).subscribe((listas: any) => {
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
  descargarDRPBoton() {
    this.disponibilidadServices.GenerateDRP(this.detailavailabilityBudget.id, this.esNovedad, this.novedadId, this.esLiberacion).subscribe((response: any) => {
      const documento = `${this.detailavailabilityBudget.numeroDRP}.pdf`;
      const text = documento,
        blob = new Blob([response], { type: 'application/pdf' }),
        anchor = document.createElement('a');
      anchor.download = documento;
      anchor.href = window.URL.createObjectURL(blob);
      anchor.dataset.downloadurl = ['application/pdf', anchor.download, anchor.href].join(':');
      anchor.click();
    });

  }

  generardrp() {
    this.disponibilidadServices.CreateDRP(this.detailavailabilityBudget.id, this.esNovedad, this.novedadId )
      .subscribe(listas => {
      console.log(listas);
      this.detailavailabilityBudget.numeroDRP = listas.data;
      //this.detailavailabilityBudget=listas;
      this.openDialog("", listas.message, true);
      if (listas.code == "200") {
        this.descargarDRPBoton();
      }
    });
  }

  generardrpPDF() {
    this.descargarDRPBoton();
  }


}
