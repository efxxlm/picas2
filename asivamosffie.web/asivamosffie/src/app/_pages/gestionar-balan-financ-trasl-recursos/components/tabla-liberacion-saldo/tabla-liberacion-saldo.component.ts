import { Component, ViewChild, OnInit, Input} from '@angular/core';
import { MatPaginator } from '@angular/material/paginator';
import { MatSort } from '@angular/material/sort';
import { MatTableDataSource } from '@angular/material/table';
import { MatDialog } from '@angular/material/dialog';
import { FormBuilder, Validators, FormArray, FormGroup } from '@angular/forms';
import { ModalDialogComponent } from 'src/app/shared/components/modal-dialog/modal-dialog.component';

@Component({
  selector: 'app-tabla-liberacion-saldo',
  templateUrl: './tabla-liberacion-saldo.component.html',
  styleUrls: ['./tabla-liberacion-saldo.component.scss']
})
export class TablaLiberacionSaldoComponent implements OnInit {
  @Input() tablaAportantes: any[];
  estaEditando: boolean;
  estadoInforme = '0';
  registroCompleto = false;
  semaforo= false;
  noGuardado=false;
  soloMostrarObservacion=false;
  displayedColumns: string[] = [
    'nombreUso',
    'nombreAportante',
    'nombreFuente',
    'valorUso',
    'saldoDisponible',
    'valorLiberar'
  ];
  listAportantes : any[] = [];
  listaUsos : any[] = [];
  //addressForm: FormGroup;
  addressForm = this.fb.group({});
  dataSource = new MatTableDataSource;

  @ViewChild(MatPaginator) paginator: MatPaginator;
  @ViewChild(MatSort) sort: MatSort;

  constructor(
    private fb: FormBuilder,
    public dialog: MatDialog) { }



  ngOnInit(): void {
    this.listaUsos = [];
    var result = this.groupListByUso(this.tablaAportantes,"codigoUso");
    this.tablaAportantes.forEach(r=>{
      if(!this.listaUsos.find(lu => lu.codigoUso == r.codigoUso)){
        this.listaUsos.push({
          codigoUso: r.codigoUso,
          nombreUso: r.nombreUso
        })
      }
    })
    this.listaUsos.forEach(r => {
        const element = result[r.codigoUso];
        if(element !== null && element != undefined){
          let list = [];
          element.forEach(dataAportante => {
              list.push({
                nombreAportante: dataAportante.nombreAportante,
                nombreFuente: dataAportante.nombreFuente,
                fuenteFinanciacionId: dataAportante.fuenteFinanciacionId,
                fuenteRecursosCodigo: dataAportante.fuenteRecursosCodigo,
                cofinanciacionAportanteId: dataAportante.cofinanciacionAportanteId,
                componenteUsoId: dataAportante.componenteUsoId,
                valorUso: dataAportante.valorUso,
                saldo: dataAportante.saldo ?? 0,
              });
          });
          this.listAportantes.push({
            codigoUso: r.codigoUso,
            nombreUso: r.nombreUso,
            data: list
          });
        }
    })
    this.dataSource.data = this.listAportantes;
  }

  applyFilter(event: Event) {
    const filterValue = (event.target as HTMLInputElement).value;
    this.dataSource.filter = filterValue.trim().toLowerCase();

    if (this.dataSource.paginator) {
      this.dataSource.paginator.firstPage();
    }
  }


  openDialog(modalTitle: string, modalText: string) {
    this.dialog.open(ModalDialogComponent, {
      width: '28em',
      data: { modalTitle, modalText },
    });
  }

  onSubmit(test: boolean) {
    this.estaEditando = true;
    this.noGuardado=false;
  }


    // evalua tecla a tecla
    validateNumberKeypress(event: KeyboardEvent) {
      const alphanumeric = /[0-9]/;
      const inputChar = String.fromCharCode(event.charCode);
      return alphanumeric.test(inputChar) ? true : false;
    }

    maxLength(e: any, n: number) {
      if (e.editor.getLength() > n) {
        e.editor.deleteText(n, e.editor.getLength());
      }
    }

    groupListByUso(list, key) {
      return list.reduce(function(rv, x) {
        (rv[x[key]] = rv[x[key]] || []).push(x);
        return rv;
      }, {});
    };

}
