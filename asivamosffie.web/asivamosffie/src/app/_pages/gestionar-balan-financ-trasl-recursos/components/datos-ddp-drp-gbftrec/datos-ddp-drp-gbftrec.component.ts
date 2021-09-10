import { Component, Input, OnInit, ViewChild } from '@angular/core';
import { MatSort } from '@angular/material/sort';
import { MatTableDataSource } from '@angular/material/table';

@Component({
  selector: 'app-datos-ddp-drp-gbftrec',
  templateUrl: './datos-ddp-drp-gbftrec.component.html',
  styleUrls: ['./datos-ddp-drp-gbftrec.component.scss']
})
export class DatosDdpDrpGbftrecComponent implements OnInit {

    @Input() solicitudPago: any;
    dataSource = new MatTableDataSource();
    displayedColumns: string[] = [
        'drp',
        'numDrp',
        'ProyectoLLaveMen',
        'NombreUso',
        'valor',
        'saldo'
    ];

    constructor() { }

    ngOnInit(): void {
        this.loadDataSource();
    }

    loadDataSource() {
        this.dataSource = new MatTableDataSource( this.solicitudPago.tablaDrpUso );
    }

}
