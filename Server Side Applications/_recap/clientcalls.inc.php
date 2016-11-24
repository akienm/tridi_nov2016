<?php

/**************************************************************************
 * Photofly RESTful api v3.0
 *
 * 2009-2011 Autodesk
 *
 **************************************************************************/

/**************************************************************************/
/**************************************************************************/
/**************************************************************************/
/**************************************************************************/
/**************************************************************************/
/**************************************************************************/
/**************************************************************************/
/**************************************************************************/
function createStringFromArray($name, $arr)
{
	$tosign = "";
	$currentIndex = 0;
	foreach($arr as $k => $v)
	{
		if (is_array($v))
			$tosign .= createStringFromArray($name . "[$currentIndex]", $v);
		else
			$tosign .= $name . "[$currentIndex]" . "=$v&";

		$currentIndex++;
	}

	return $tosign;
}

function signIt($relativurl, &$data, $method, $type = "pf")
{
	GLOBAL $access;

	// in our case we need to urldecode the URL in case the ID is encoded
	$relativurl = rtrim(urldecode($relativurl), "/");

	$toSign = "/" . $relativurl;
	$clientID = $data["clientID"];

	//if ($method == "get")
		$toSign .= "?";

	$files_sign = "";
	//print_r($data);
	foreach($data as $name => $value)
	{
		if ($name != "signature")
		{
			// if it s a file
			if ($value[0] == "@")
			{
				$fname = trim($value, "@");
				//$files_sign .= sha1_file($fname);
			}
			else
			{
				if (is_array($value))
                {
                	$toSign .= createStringFromArray($name, $value);
                	continue;
                }

				$toSign .= $name . "=" . $value . "&";
			}
		}
	}

	$toSign = trim($toSign, "&");

	$token = $access ['oauth_token'];
	$secret = $access ['oauth_token_secret'];

	if ($token == "" || $secret == "")
	{
		echo "Error must be O2 logged first<br>";
		return "";
	}
	$oauth = new OAuth(ConsumerKey,ConsumerSecret,OAUTH_SIG_METHOD_HMACSHA1);
    $oauth->setToken($token, $secret);

    $url = "http://rc-api-adn.autodesk.com/3.0/API" . $toSign . $files_sign;
    //echo "To OAUTH sign: $method, $url<br/>";
	return $oauth->getRequestHeader(strtoupper($method), $url);
}


function http_custom_build_query($data, $pref, $sep)
{
	if (count($data) > 0)
	{	$co = "";
		foreach($data as $key => $value)
		{
			$q .= $co . urlencode($key) . "=" . urlencode($value);
			$co = $sep;
		}
	}

	return $q;
}

// LOCAL FUNCTION
function CURLcall($data, $resource, $method, $url="")
{
	$authorization = signIt($resource, $data, $method);
	if ($authorization == "")
	{
		echo "No authorization\n";
		return;
	}
   
     // create a new cURL resource
    $cURL = curl_init();
    curl_setopt($cURL, CURLOPT_USERAGENT, "SoundfitUserAgent");

    $url .= $resource;

    // set URL and other appropriate options
    switch($method)
    {
        case "post":
            curl_setopt($cURL, CURLOPT_POST, TRUE);
            curl_setopt($cURL, CURLOPT_POSTFIELDS, $data);
        break;
        case "get":
            curl_setopt($cURL, CURLOPT_HTTPGET, TRUE);

            if (!empty($data))
                $url = $url . '?' . http_custom_build_query($data, '', '&');
        break;
        case "delete":
            if (!empty($data))
                $d = http_custom_build_query($data, '', '&');

            curl_setopt($cURL, CURLOPT_CUSTOMREQUEST, "DELETE");
            curl_setopt($cURL, CURLOPT_POSTFIELDS, $d);
        break;
        case "put":
            if (!empty($data))
                $d = http_custom_build_query($data, '', '&');

            curl_setopt($cURL, CURLOPT_CUSTOMREQUEST, "PUT");
            curl_setopt($cURL, CURLOPT_POSTFIELDS, $d);

            //curl_setopt($cURL, CURLOPT_PUT, true);
            //curl_setopt($cURL, CURLOPT_POSTFIELDS, $d);


            //echo "la$d";

            /*$fh  = fopen('php://memory');
            fwrite($fh, $d);
            rewind($fh);

            curl_setopt($cURL, CURLOPT_PUT, true);
            curl_setopt($cURL, CURLOPT_INFILE, $fh);
            curl_setopt($cURL, CURLOPT_INFILESIZE, strlen($d));
            curl_setopt($cURL, CURLOPT_RETURNTRANSFER, true);

            fclose($fh);
             */
        break;
        default:
            return "Error with client request !";
        break;
    }

    curl_setopt($cURL, CURLOPT_URL, $url);
    curl_setopt($cURL, CURLOPT_RETURNTRANSFER, TRUE);
    curl_setopt($cURL, CURLOPT_HEADER, 0);
	//curl_setopt($cURL, CURLINFO_HEADER_OUT, true);

   	curl_setopt($cURL,CURLOPT_HTTPHEADER,array("Authorization: $authorization"));

    curl_setopt($cURL, CURLOPT_TIMEOUT, 100);
    //curl_setopt($cURL, CURLOPT_VERBOSE, TRUE);

    //$fp = fopen("output.txt", "w");
    //curl_setopt($cURL, CURLOPT_INFILE, $fp);


    // grab URL and pass it to the browser
    $res = curl_exec($cURL);

    //fclose($fp);

    $info = curl_getinfo($cURL);

	// close cURL resource, and free up system resources
    curl_close($cURL);

    return $res;
}


?>
