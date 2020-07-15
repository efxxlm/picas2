import { Component, OnInit, ViewChild } from '@angular/core';
import { MatPaginator } from '@angular/material/paginator';
import { MatSort } from '@angular/material/sort';
import { MatTableDataSource } from '@angular/material/table';
import { CofinanciacionService, Cofinanciacion } from 'src/app/core/_services/Cofinanciacion/cofinanciacion.service';
import { Router } from '@angular/router';
import { Respuesta } from 'src/app/core/_services/common/common.service';
import { ModalDialogComponent } from 'src/app/shared/components/modal-dialog/modal-dialog.component';
import { MatDialog } from '@angular/material/dialog';

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
               public dialog: MatDialog ) { }

  ngOnInit(): void {

    this.cofinanciacionService.listaAcuerdosCofinanciacion().subscribe( cof => 
      {
         this.listaCofinanciacion = cof; 
         this.dataSource.data = this.listaCofinanciacion;
         this.dataSource.sort = this.sort;
        this.dataSource.paginator = this.paginator;
        this.paginator._intl.itemsPerPageLabel = 'Elementos por página';
        this.paginator._intl.nextPageLabel = 'Siguiente';
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
      this.openDialog('', respuesta.message);
      this.ngOnInit();
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
        this.cofinanciacionService.getAcuerdoCofinanciacionById(e).subscribe(cofi => {
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
        });
      }           
    });
  }

}
