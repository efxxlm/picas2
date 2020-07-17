import { Component, OnInit, ViewChild } from '@angular/core';
import { MatPaginator } from '@angular/material/paginator';
import { MatSort } from '@angular/material/sort';
import { MatTableDataSource } from '@angular/material/table';
import { CofinanciacionService, Cofinanciacion } from 'src/app/core/_services/Cofinanciacion/cofinanciacion.service';
import { Router } from '@angular/router';
import { Respuesta } from 'src/app/core/_services/common/common.service';
import { ModalDialogComponent } from 'src/app/shared/components/modal-dialog/modal-dialog.component';
import { MatDialog } from '@angular/material/dialog';
import { DomSanitizer } from '@angular/platform-browser';

// export interface PeriodicElement {
//   id: number;
//   fechaCreacion: string;
//   numeroAcuerdo: string;
//   vigenciaAcuerdo: number;
//   valorTotal: number;
//   estadoRegistro: string;
// }

// const ELEMENT_DATA: PeriodicElement[] = [
//   {id: 1, fechaCreacion: '26/05/2020', numeroAcuerdo: '000001', vigenciaAcuerdo: 2020, valorTotal: 85000000, estadoRegistro: 'Completo'},
//   {id: 2, fechaCreacion: '26/05/2020', numeroAcuerdo: '000001', vigenciaAcuerdo: 2020, valorTotal: 85000000, estadoRegistro: 'Completo'},
//   {id: 3, fechaCreacion: '26/05/2020', numeroAcuerdo: '000001', vigenciaAcuerdo: 2020, valorTotal: 85000000, estadoRegistro: 'Completo'},
// ];

@Component({
  selector: 'app-tabla-acuerdos',
  templateUrl: './tabla-acuerdos.component.html',
  styleUrls: ['./tabla-acuerdos.component.scss']
})
export class TablaAcuerdosComponent implements OnInit {

  displayedColumns: string[] = ['fechaCreacion', 'numeroAcuerdo', 'vigenciaAcuerdo', 'valorTotal', 'estadoRegistro', 'id'];
  dataSource = new MatTableDataSource();
  listaCofinanciacion: Cofinanciacion[] = [];

  @ViewChild(MatPaginator, { static: true }) paginator: MatPaginator;
  @ViewChild(MatSort, {static: true}) sort: MatSort;

  applyFilter(event: Event) {
    const filterValue = (event.target as HTMLInputElement).value;
    this.dataSource.filter = filterValue.trim().toLowerCase();
  }

  constructor( private cofinanciacionService: CofinanciacionService,
               private router: Router,
               public dialog: MatDialog,private sanitized: DomSanitizer ) { }

  ngOnInit(): void {
    this.inicializarTabla();        
  }
  inicializarTabla(){
    this.cofinanciacionService.listaAcuerdosCofinanciacion().subscribe( cof => 
      {
         this.listaCofinanciacion = cof; 
         this.listaCofinanciacion.forEach(element => {
           element.valorTotal=0;
           element.estadoRegistro="Completo";
           if(element.cofinanciacionAportante.length==0)
           {
            element.estadoRegistro="Incompleto";
           }
           element.cofinanciacionAportante.forEach(elementaportante => {
            if(elementaportante.cofinanciacionDocumento.length==0)
            {
             element.estadoRegistro="Incompleto";
            }
            elementaportante.cofinanciacionDocumento.forEach(documento => {
               element.valorTotal+=documento.valorDocumento;
             });
           });
           
         });
         this.dataSource.data = this.listaCofinanciacion;
         this.dataSource.sort = this.sort;
        this.dataSource.paginator = this.paginator;
        this.paginator._intl.itemsPerPageLabel = 'Elementos por página';
        this.paginator._intl.nextPageLabel = 'Siguiente';
        this.paginator._intl.getRangeLabel = function (page, pageSize, length) {   
          /*let pages="";
          for(let i =0; i<=length;i++)
          {
            if(i==page)
            {
              pages="<div class='pag-actual'>"+(i+1)+"</div>";  
            }
            else{
              pages="<div class='pag'>"+(i+1)+"</div>";
            }            
          }     
          return this.sanitized.bypassSecurityTrustHtml(pages);*/
          if (length === 0 || pageSize === 0) {
            return '0 de ' + length;
          }
          length = Math.max(length, 0);
          const startIndex = page * pageSize;
          // If the start index exceeds the list length, do not try and fix the end index to the end.
          const endIndex = startIndex < length ?
            Math.min(startIndex + pageSize, length) :
            startIndex + pageSize;
          return startIndex + 1 + ' - ' + endIndex + ' de ' + length;
        };
        this.paginator._intl.previousPageLabel = 'Anterior';
      } );
  }

  editarAcuerdo(e: number) {
    this.router.navigate(['/registrarAcuerdos', e ]);
  }

   eliminarAcuerdo(e: number) {
    this.openDialogSiNo("", "<b>¿Está seguro de eliminar este registro?</b>", e);
    /*this.cofinanciacionService.getAcuerdoCofinanciacionById(e).subscribe(cofi => {
      cofi.eliminado = true;
      cofi.cofinanciacionAportante.forEach( apo => {
          apo.eliminado = true;
          apo.cofinanciacionDocumento.forEach( doc => {
             doc.eliminado = true; 
          });
        })

        console.log(cofi);

    this.cofinanciacionService.CrearOModificarAcuerdoCofinanciacion(cofi).subscribe( 
      respuesta => 
      {
        this.verificarRespuesta( respuesta );
      },
      err => {
        let mensaje: string;
        if (err.error.message){
          mensaje = err.error.message;
        }else {
          mensaje = err.message;
        }
        this.openDialog('Error', mensaje);
     },
     () => {
      //console.log('terminó');
     });
    });*/
   }

  private verificarRespuesta( respuesta: Respuesta )
  {
    if (respuesta.isSuccessful) // Response witout errors
    {
      this.inicializarTabla();
      this.openDialog('', respuesta.message);
      
      if (respuesta.isValidation) // have validations
      {
        
      }
     }else{
      this.openDialog('', respuesta.message);
    }
  }

  openDialog(modalTitle: string, modalText: string) {
    let dialogRef =this.dialog.open(ModalDialogComponent, {
      width: '28em',
      data: { modalTitle, modalText }
    });   
  }

  openDialogSiNo(modalTitle: string, modalText: string, e:number) {
    let dialogRef =this.dialog.open(ModalDialogComponent, {
      width: '28em',
      data: { modalTitle, modalText, siNoBoton:true }
    });   
    dialogRef.afterClosed().subscribe(result => {
      console.log(`Dialog result: ${result}`);
      if(result)
      {
        this.cofinanciacionService.EliminarCofinanciacionByCofinanciacionId(e).subscribe(respuesta => {
            console.log(respuesta);
            this.verificarRespuesta( respuesta );
          },
          err => {
            let mensaje: string;
            if (err.error.message){
              mensaje = err.error.message;
            }else {
              mensaje = err.message;
            }
            this.openDialog('Error', mensaje);
         },
         () => {
          //console.log('terminó');
         });
      }           
    });
  }

}
