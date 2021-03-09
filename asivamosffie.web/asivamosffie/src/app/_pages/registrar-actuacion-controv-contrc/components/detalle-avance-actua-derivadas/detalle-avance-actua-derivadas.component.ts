import { Component, OnInit } from '@angular/core';
import { FormBuilder } from '@angular/forms';
import { MatDialog } from '@angular/material/dialog';
import { Router, ActivatedRoute } from '@angular/router';
import { CommonService } from 'src/app/core/_services/common/common.service';
import { ContractualControversyService } from 'src/app/core/_services/ContractualControversy/contractual-controversy.service';

@Component({
  selector: 'app-detalle-avance-actua-derivadas',
  templateUrl: './detalle-avance-actua-derivadas.component.html',
  styleUrls: ['./detalle-avance-actua-derivadas.component.scss']
})
export class DetalleAvanceActuaDerivadasComponent implements OnInit {
  controversiaID: any;
  actuacionDerivadaID: any;
  controversia: any;
  actuacionDerivadaInfo: any;
  constructor(private fb: FormBuilder,private router: Router, private conServices:ContractualControversyService,
    public dialog: MatDialog,
    public commonServices: CommonService,
    private activatedRoute: ActivatedRoute,)
     { }

  ngOnInit(): void {
    this.activatedRoute.params.subscribe( param => {
      this.controversiaID = param['id'];
      this.actuacionDerivadaID = param['editId'];
      this.conServices.GetControversiaActuacionById(this.controversiaID).subscribe(
        response=>{
          this.controversia=response;          
        }
      );
      this.conServices.GetSeguimientoActuacionDerivadabyId(this.actuacionDerivadaID).subscribe((data:any)=>{
        this.actuacionDerivadaInfo = data;
      });
    });
  }

}