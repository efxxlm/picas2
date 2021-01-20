import { Component, OnInit } from '@angular/core';
import { MatTableDataSource } from '@angular/material/table';
import { MatDialog, MatDialogRef, MatDialogConfig, MAT_DIALOG_DATA } from '@angular/material/dialog';
import { CancelarDrpComponent } from '../cancelar-drp/cancelar-drp.component';
import { DisponibilidadPresupuestalService } from 'src/app/core/_services/disponibilidadPresupuestal/disponibilidad-presupuestal.service';
import { ActivatedRoute, Router } from '@angular/router';
import { DomSanitizer } from '@angular/platform-browser';
import { ModalDialogComponent } from 'src/app/shared/components/modal-dialog/modal-dialog.component';
import { CurrencyPipe } from '@angular/common';

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
  listacomponentes:tablaEjemplo[]=[];
  public numContrato = "";
  public fechaContrato = "";
  public solicitudContrato = "";
  public estadoSolicitud = "";
  displayedColumns: string[] = ['componente', 'uso', 'valorUso', 'valorTotal'];
  esModificacion=false;
  dataSource = [];
  detailavailabilityBudget: any;
  constructor(public dialog: MatDialog,private disponibilidadServices: DisponibilidadPresupuestalService,
    private route: ActivatedRoute,private currencyPipe:CurrencyPipe,
    private router: Router,private sanitized: DomSanitizer,) { }
  
    openDialog(modalTitle: string, modalText: string,relocate=false) {
      let dialogref=this.dialog.open(ModalDialogComponent, {
        width: '28em',
        data: { modalTitle, modalText }
      });
      if(relocate)
      {
        dialogref.afterClosed().subscribe(result => {
          this.router.navigate(["/generarRegistroPresupuestal"], {});
         });
      }
    }
  ngOnInit(): void {
    const id = this.route.snapshot.paramMap.get('id');
    if (id) {
      this.disponibilidadServices.GetDetailAvailabilityBudgetProyect(id).subscribe(listas => {
        console.log(listas);
        console.log(listas);
        if(listas.length>0)
        {
          this.detailavailabilityBudget=listas[0];
          this.detailavailabilityBudget.proyectos.forEach(element => {
            element.componenteGrilla.forEach(element2 => {                          
              this.listacomponentes.push({
                componente: element2.componente, uso: [
                  { nombre: element2.uso }//, { nombre: "Diagnostico" }, { nombre: "Obra Principal" }
                ], valorUso: [
                  { valor: element2.valorUso.map(y=>{let convert=y.toString().replace(/(\d)(?=(\d{3})+(?!\d))/g, "$1,"); return "$"+convert;})}//, { valor: "$ 12.000.000" }, { valor: "$ 60.000.000" }
                ], valorTotal: element2.valorTotal
              });
            });
            this.dataSource.push(new MatTableDataSource(this.listacomponentes));
          });
          
        }
        else{
          this.openDialog('','Error al intentar recuperar los datos de la solicitud, por favor intenta nuevamente.');
        }
      });
    }
    //this.dataSource = new MatTableDataSource(ELEMENT_DATA);

  }

  
  cancelarDRPBoton(){
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
  
  descargarDDPBoton(){    
    console.log(this.detailavailabilityBudget);
    this.disponibilidadServices.GenerateDDP(this.detailavailabilityBudget.id).subscribe((listas:any) => {
      console.log(listas);
      const documento = `${ this.detailavailabilityBudget.numeroDDP  }.pdf`;
        const text = documento,
          blob = new Blob([listas], { type: 'application/pdf' }),
          anchor = document.createElement('a');
        anchor.download = documento;
        anchor.href = window.URL.createObjectURL(blob);
        anchor.dataset.downloadurl = ['application/pdf', anchor.download, anchor.href].join(':');
        anchor.click();
    });
  
  }
  descargarDRPBoton(){    
    console.log(this.detailavailabilityBudget);
    this.disponibilidadServices.GenerateDRP(this.detailavailabilityBudget.id).subscribe((listas:any) => {
      console.log(listas);
      const documento = `DRP ${ this.detailavailabilityBudget.id }.pdf`;
        const text = documento,
          blob = new Blob([listas], { type: 'application/pdf' }),
          anchor = document.createElement('a');
        anchor.download = documento;
        anchor.href = window.URL.createObjectURL(blob);
        anchor.dataset.downloadurl = ['application/pdf', anchor.download, anchor.href].join(':');
        anchor.click();
    });
  
  }

  generardrp(){
    this.disponibilidadServices.CreateDRP(this.detailavailabilityBudget.id).subscribe(listas => {
      console.log(listas);
      //this.detailavailabilityBudget=listas;
      this.openDialog("",listas.message,true);
      if(listas.code=="200")
      {
        this.descargarDRPBoton();
      } 
    });
  }

  generardrpPDF()
  {
    this.descargarDRPBoton();
  } 
  

}
